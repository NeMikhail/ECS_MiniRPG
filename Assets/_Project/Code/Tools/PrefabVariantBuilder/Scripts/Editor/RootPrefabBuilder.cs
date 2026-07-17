using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MAEngine.PrefabVariantBuilder
{
    internal static class RootPrefabBuilder
    {
        internal static string Build(AnalysisResult analysis, string rootName, string rootAssetPath)
        {
            var assetFolder = rootAssetPath.Substring(0, rootAssetPath.LastIndexOf('/'));
            EnsureDirectory(assetFolder);

            var rootGo = new GameObject(rootName);
            try
            {
                // Add root-level shared components
                var rootEntries = analysis.SharedEntries
                    .Where(e => e.TransformPath == string.Empty)
                    .ToList();

                foreach (var entry in rootEntries)
                    AddComponentFromSnapshot(rootGo, entry.Representative);

                // Build shared child hierarchy grouped by path
                var childEntries = analysis.SharedEntries
                    .Where(e => e.TransformPath != string.Empty)
                    .ToList();

                // Collect all unique paths that need to be created
                var pathsToCreate = childEntries
                    .Select(e => e.TransformPath)
                    .Distinct()
                    .OrderBy(p => p) // shortest first ensures parents created before children
                    .ToList();

                foreach (var path in pathsToCreate)
                    EnsureChildExists(rootGo.transform, path);

                // Add shared components to children
                foreach (var entry in childEntries)
                {
                    var childTransform = rootGo.transform.Find(entry.TransformPath);
                    if (childTransform == null)
                    {
                        Debug.LogWarning($"[PrefabVariantBuilder] Could not find child at path '{entry.TransformPath}' in root");
                        continue;
                    }
                    AddComponentFromSnapshot(childTransform.gameObject, entry.Representative);
                }

                PrefabUtility.SaveAsPrefabAsset(rootGo, rootAssetPath);
                return rootAssetPath;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(rootGo);
            }
        }

        private static void AddComponentFromSnapshot(GameObject go, ComponentSnapshot snapshot)
        {
            var type = Type.GetType(snapshot.TypeAssemblyQualified);
            if (type == null)
            {
                Debug.LogWarning($"[PrefabVariantBuilder] Cannot resolve type: {snapshot.TypeAssemblyQualified}");
                return;
            }

            // GameObject sentinel — apply GO-level properties (m_Layer, m_TagString, etc.) directly;
            // there is no "AddComponent" for the GameObject itself.
            if (type == typeof(GameObject))
            {
                ApplySnapshot(go, snapshot);
                return;
            }

            // Transform sentinel — always present on every GO, cannot be added via AddComponent.
            // Apply layout properties (position, rotation, scale) directly to the existing Transform.
            if (type == typeof(Transform))
            {
                ApplySnapshot(go.transform, snapshot);
                return;
            }

            // Check for duplicate component type
            if (go.GetComponent(type) != null)
            {
                Debug.LogWarning($"[PrefabVariantBuilder] Component {snapshot.TypeDisplayName} already exists on {go.name}, skipping duplicate.");
                return;
            }

            var comp = go.AddComponent(type);
            ApplySnapshot(comp, snapshot);
        }

        // Accepts both Component and GameObject (both derive from UnityEngine.Object).
        internal static void ApplySnapshot(UnityEngine.Object target, ComponentSnapshot snapshot)
        {
            var so = new SerializedObject(target);
            ApplyPropertiesWithArrayFlush(so, snapshot.Properties);
        }

        internal static void ApplyPropertiesWithArrayFlush(SerializedObject so, List<PropertySnapshot> properties)
        {
            foreach (var propSnap in properties
                .Where(p => p.PropertyPath.EndsWith(".Array.size"))
                .OrderBy(p => p.PropertyPath.Length))
            {
                so.Update();
                var prop = so.FindProperty(propSnap.PropertyPath);
                if (prop == null) continue;
                PropertyValueResolver.Apply(prop, propSnap);
                so.ApplyModifiedProperties();
            }
            so.Update();
            foreach (var propSnap in properties.Where(p => !p.PropertyPath.EndsWith(".Array.size")))
            {
                var prop = so.FindProperty(propSnap.PropertyPath);
                if (prop == null) continue;
                PropertyValueResolver.Apply(prop, propSnap);
            }
            so.ApplyModifiedProperties();
        }

        private static void EnsureChildExists(Transform parent, string relativePath)
        {
            var segments = relativePath.Split('/');
            var current = parent;
            foreach (var segment in segments)
            {
                var found = current.Find(segment);
                if (found == null)
                {
                    var childGo = new GameObject(segment);
                    childGo.transform.SetParent(current, false);
                    current = childGo.transform;
                }
                else
                {
                    current = found;
                }
            }
        }

        private static void EnsureDirectory(string assetFolder)
        {
            // Convert asset-relative path to absolute
            var absolutePath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetFolder));
            Directory.CreateDirectory(absolutePath);
        }
    }
}
