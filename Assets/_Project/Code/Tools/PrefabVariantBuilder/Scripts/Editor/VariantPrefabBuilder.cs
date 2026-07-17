using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MAEngine.PrefabVariantBuilder
{
    internal static class VariantPrefabBuilder
    {
        internal static List<string> BuildAll(AnalysisResult analysis, string rootPrefabAssetPath, string outputAssetFolder, bool replaceMode, string rootSourcePath)
        {
            var rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rootPrefabAssetPath);
            if (rootPrefab == null)
            {
                Debug.LogError($"[PrefabVariantBuilder] Cannot load root prefab at: {rootPrefabAssetPath}");
                return new List<string>();
            }

            var createdPaths = new List<string>();
            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var prefabsToProcess = replaceMode && !string.IsNullOrEmpty(rootSourcePath)
                ? analysis.AllPrefabs.Where(p => p.AssetPath != rootSourcePath).ToList()
                : analysis.AllPrefabs;

            foreach (var prefabData in prefabsToProcess)
            {
                var variantName = ResolveUniqueName(prefabData.PrefabName, usedNames);
                usedNames.Add(variantName);

                var variantPath = replaceMode
                    ? prefabData.AssetPath
                    : outputAssetFolder + "/" + variantName + ".prefab";
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(rootPrefab);
                instance.name = variantName;

                try
                {
                    ApplyOverrides(instance, analysis, prefabData);
                    AddUniqueComponents(instance, analysis, prefabData);
                    AddUniquePaths(instance, analysis, prefabData);
                    RemoveAbsentComponents(instance, analysis, prefabData);

                    PrefabUtility.SaveAsPrefabAsset(instance, variantPath);
                    createdPaths.Add(variantPath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[PrefabVariantBuilder] Failed to build variant for '{prefabData.PrefabName}': {e}");
                }
                finally
                {
                    UnityEngine.Object.DestroyImmediate(instance);
                }
            }

            return createdPaths;
        }

        private static void ApplyOverrides(GameObject instance, AnalysisResult analysis, PrefabData prefabData)
        {
            if (!analysis.PrefabOverrides.TryGetValue(prefabData.AssetPath, out var overrides)) return;

            foreach (var variantOverride in overrides)
            {
                var go = FindGoAtPath(instance, variantOverride.TransformPath);
                if (go == null)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] Override: cannot find GO at '{variantOverride.TransformPath}'");
                    continue;
                }

                var type = Type.GetType(variantOverride.TypeAssemblyQualified);
                if (type == null) continue;

                // For the GameObject sentinel use the GO itself; otherwise get the component.
                UnityEngine.Object target = type == typeof(GameObject)
                    ? (UnityEngine.Object)go
                    : go.GetComponent(type);
                if (target == null) continue;

                var so = new SerializedObject(target);
                RootPrefabBuilder.ApplyPropertiesWithArrayFlush(so, variantOverride.DifferingProps);
            }
        }

        private static void AddUniqueComponents(GameObject instance, AnalysisResult analysis, PrefabData prefabData)
        {
            if (!analysis.UniqueComponents.TryGetValue(prefabData.AssetPath, out var uniqueComps)) return;

            foreach (var (path, compSnapshot) in uniqueComps)
            {
                var go = FindGoAtPath(instance, path);
                if (go == null)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] UniqueComp: cannot find GO at '{path}'");
                    continue;
                }

                var type = Type.GetType(compSnapshot.TypeAssemblyQualified);
                if (type == null)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] Cannot resolve type: {compSnapshot.TypeAssemblyQualified}");
                    continue;
                }

                // GameObject and Transform sentinels are always present — apply properties directly.
                if (type == typeof(GameObject))
                {
                    RootPrefabBuilder.ApplySnapshot(go, compSnapshot);
                    continue;
                }
                if (type == typeof(Transform))
                {
                    RootPrefabBuilder.ApplySnapshot(go.transform, compSnapshot);
                    continue;
                }

                if (go.GetComponent(type) != null)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] Component {compSnapshot.TypeDisplayName} already exists on {go.name}, skipping.");
                    continue;
                }

                var comp = go.AddComponent(type);
                RootPrefabBuilder.ApplySnapshot(comp, compSnapshot);
            }
        }

        private static void AddUniquePaths(GameObject instance, AnalysisResult analysis, PrefabData prefabData)
        {
            if (!analysis.UniquePaths.TryGetValue(prefabData.AssetPath, out var uniquePaths)) return;

            foreach (var pathNode in uniquePaths)
                CreateHierarchyNode(instance.transform, pathNode);
        }

        private static void CreateHierarchyNode(Transform parent, HierarchyNode node)
        {
            // Determine the local name of this node (last segment of the path)
            var name = node.TransformPath.Contains('/')
                ? node.TransformPath.Substring(node.TransformPath.LastIndexOf('/') + 1)
                : node.TransformPath;

            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            foreach (var compSnapshot in node.Components)
            {
                var type = Type.GetType(compSnapshot.TypeAssemblyQualified);
                if (type == null)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] Cannot resolve type: {compSnapshot.TypeAssemblyQualified}");
                    continue;
                }

                // Sentinels: apply to the target directly, AddComponent is invalid for both.
                if (type == typeof(GameObject))
                {
                    RootPrefabBuilder.ApplySnapshot(go, compSnapshot);
                    continue;
                }
                if (type == typeof(Transform))
                {
                    RootPrefabBuilder.ApplySnapshot(go.transform, compSnapshot);
                    continue;
                }

                var comp = go.AddComponent(type);
                RootPrefabBuilder.ApplySnapshot(comp, compSnapshot);
            }

            foreach (var child in node.Children)
                CreateHierarchyNode(go.transform, child);
        }

        private static void RemoveAbsentComponents(GameObject instance, AnalysisResult analysis, PrefabData prefabData)
        {
            if (!analysis.RemovedEntries.TryGetValue(prefabData.AssetPath, out var removed)) return;

            foreach (var (path, typeName) in removed)
            {
                var go = FindGoAtPath(instance, path);
                if (go == null) continue;

                var type = Type.GetType(typeName);
                if (type == null) continue;

                // GameObject and Transform sentinels cannot be removed.
                if (type == typeof(GameObject) || type == typeof(Transform)) continue;

                var comp = go.GetComponent(type);
                if (comp == null) continue;

                // Destroying the component on the prefab instance creates a "removed component"
                // override. SaveAsPrefabAsset (called by the caller) will persist this override
                // into the variant file. ApplyRemovedComponent is the wrong API here — it would
                // push the removal back into the source (root) prefab.
                try
                {
                    Object.DestroyImmediate(comp);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] Cannot remove {type.Name} from '{path}': {e.Message}");
                }
            }
        }

        private static GameObject FindGoAtPath(GameObject root, string path)
        {
            if (string.IsNullOrEmpty(path)) return root;
            var t = root.transform.Find(path);
            return t != null ? t.gameObject : null;
        }

        private static string ResolveUniqueName(string baseName, HashSet<string> usedNames)
        {
            if (!usedNames.Contains(baseName)) return baseName;
            var counter = 1;
            string candidate;
            do { candidate = baseName + "_" + counter++; }
            while (usedNames.Contains(candidate));
            return candidate;
        }
    }
}
