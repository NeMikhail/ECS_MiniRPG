using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MAEngine.PrefabVariantBuilder
{
    internal static class PrefabAnalyzer
    {
        internal static AnalysisResult Analyze(List<string> assetPaths, float threshold)
        {
            var result = new AnalysisResult();

            // Stage A: traverse all prefabs
            foreach (var path in assetPaths)
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null)
                {
                    Debug.LogWarning($"[PrefabVariantBuilder] Cannot load prefab at: {path}");
                    continue;
                }

                var data = new PrefabData
                {
                    AssetPath = path,
                    PrefabName = Path.GetFileNameWithoutExtension(path),
                    Root = PrefabHierarchyTraverser.Traverse(go)
                };
                result.AllPrefabs.Add(data);
            }

            var total = result.AllPrefabs.Count;
            if (total == 0) return result;

            // Stage B: count frequency of each (path, componentType) pair and collect all values.
            // key = "transformPath|TypeAssemblyQualified"
            var frequency = new Dictionary<string, int>();
            // All per-prefab snapshots for each key (for majority-vote in Stage C).
            var allValues = new Dictionary<string, List<(string PrefabPath, ComponentSnapshot Snap)>>();
            // Track which prefab paths had each key to count each prefab only once.
            var prefabsWithKey = new Dictionary<string, HashSet<string>>();

            foreach (var prefabData in result.AllPrefabs)
            {
                CollectFrequency(prefabData.Root, prefabData.AssetPath, frequency, allValues, prefabsWithKey);
            }

            // Stage C: classify SharedEntries (freq >= threshold).
            // The representative is chosen by majority vote: the snapshot value shared by the most
            // prefabs becomes the root-prefab value, minimising the number of variant overrides.
            // Ties are broken by alphabetical prefab path for determinism across runs.
            var sharedKeys = new HashSet<string>();
            // Built in the same pass to avoid calling PickMajorityRepresentative twice per key.
            var representativesLookup = new Dictionary<string, ComponentSnapshot>();
            foreach (var kvp in frequency)
            {
                var freq = (float)kvp.Value / total;
                if (freq >= threshold)
                {
                    sharedKeys.Add(kvp.Key);
                    var representative = PickMajorityRepresentative(allValues[kvp.Key]);
                    representativesLookup[kvp.Key] = representative;
                    result.SharedEntries.Add(new SharedEntry
                    {
                        TransformPath = ExtractPath(kvp.Key),
                        Representative = representative
                    });
                }
            }

            // Sort SharedEntries: root first, then by path depth, then alphabetically
            result.SharedEntries.Sort((a, b) =>
            {
                if (a.TransformPath == b.TransformPath)
                    return string.Compare(a.Representative.TypeDisplayName, b.Representative.TypeDisplayName, StringComparison.Ordinal);
                return string.Compare(a.TransformPath, b.TransformPath, StringComparison.Ordinal);
            });

            // Stage D: per-prefab classification
            foreach (var prefabData in result.AllPrefabs)
            {
                var overrides = new List<VariantOverride>();
                var uniqueComps = new List<(string, ComponentSnapshot)>();
                var uniquePaths = new List<HierarchyNode>();
                var removed = new List<(string, string)>();

                ClassifyPrefab(prefabData, prefabData.Root, sharedKeys, representativesLookup,
                    overrides, uniqueComps, uniquePaths, removed);

                result.PrefabOverrides[prefabData.AssetPath] = overrides;
                result.UniqueComponents[prefabData.AssetPath] = uniqueComps;
                result.UniquePaths[prefabData.AssetPath] = uniquePaths;
                result.RemovedEntries[prefabData.AssetPath] = removed;
            }

            return result;
        }

        private static void CollectFrequency(
            HierarchyNode node,
            string prefabAssetPath,
            Dictionary<string, int> frequency,
            Dictionary<string, List<(string PrefabPath, ComponentSnapshot Snap)>> allValues,
            Dictionary<string, HashSet<string>> prefabsWithKey)
        {
            foreach (var comp in node.Components)
            {
                var key = MakeKey(node.TransformPath, comp.TypeAssemblyQualified);
                if (!frequency.ContainsKey(key))
                {
                    frequency[key] = 0;
                    allValues[key] = new List<(string, ComponentSnapshot)>();
                    prefabsWithKey[key] = new HashSet<string>();
                }
                // Only count once per prefab; but still record the snapshot value.
                if (prefabsWithKey[key].Add(prefabAssetPath))
                {
                    frequency[key]++;
                    allValues[key].Add((prefabAssetPath, comp));
                }
            }

            foreach (var child in node.Children)
                CollectFrequency(child, prefabAssetPath, frequency, allValues, prefabsWithKey);
        }

        private static void ClassifyPrefab(
            PrefabData prefabData,
            HierarchyNode node,
            HashSet<string> sharedKeys,
            Dictionary<string, ComponentSnapshot> representatives,
            List<VariantOverride> overrides,
            List<(string, ComponentSnapshot)> uniqueComps,
            List<HierarchyNode> uniquePaths,
            List<(string, string)> removed)
        {
            // Build lookup of component types present in this node
            var presentTypes = new HashSet<string>(node.Components.Select(c => c.TypeAssemblyQualified));

            // Check each shared key at this path
            var keysAtPath = sharedKeys.Where(k => ExtractPath(k) == node.TransformPath).ToList();
            foreach (var key in keysAtPath)
            {
                var typeName = ExtractType(key);
                if (!presentTypes.Contains(typeName))
                {
                    // Component is shared (in root) but this prefab doesn't have it → remove in variant
                    removed.Add((node.TransformPath, typeName));
                }
                else
                {
                    // Component is shared and present → check for property overrides
                    var comp = node.Components.First(c => c.TypeAssemblyQualified == typeName);
                    var representative = representatives[key];
                    var diffProps = FindDifferingProperties(comp, representative);
                    if (diffProps.Count > 0)
                    {
                        overrides.Add(new VariantOverride
                        {
                            TransformPath = node.TransformPath,
                            TypeAssemblyQualified = typeName,
                            DifferingProps = diffProps
                        });
                    }
                }
            }

            // Check for unique components on this node (not shared, but present here)
            foreach (var comp in node.Components)
            {
                var key = MakeKey(node.TransformPath, comp.TypeAssemblyQualified);
                if (!sharedKeys.Contains(key))
                    uniqueComps.Add((node.TransformPath, comp));
            }

            // Check children: if a child path is not covered by any shared key at any depth,
            // it's a unique path for this variant
            foreach (var child in node.Children)
            {
                var childHasAnyShared = sharedKeys.Any(k => ExtractPath(k) == child.TransformPath
                    || ExtractPath(k).StartsWith(child.TransformPath + "/", StringComparison.Ordinal));

                if (!childHasAnyShared)
                {
                    // Entire subtree is unique to this prefab
                    uniquePaths.Add(child);
                }
                else
                {
                    // Path is shared — recurse into it
                    ClassifyPrefab(prefabData, child, sharedKeys, representatives,
                        overrides, uniqueComps, uniquePaths, removed);
                }
            }
        }

        private static List<PropertySnapshot> FindDifferingProperties(
            ComponentSnapshot variant, ComponentSnapshot representative)
        {
            var result = new List<PropertySnapshot>();
            var repLookup = new Dictionary<string, PropertySnapshot>();
            foreach (var p in representative.Properties)
                repLookup[p.PropertyPath] = p;

            foreach (var prop in variant.Properties)
            {
                if (!repLookup.TryGetValue(prop.PropertyPath, out var repProp) ||
                    !PropertyValueResolver.AreEqual(prop, repProp))
                    result.Add(prop);
            }
            return result;
        }

        // Picks the ComponentSnapshot whose property values are shared by the largest number of
        // prefabs in the list. Ties are broken by alphabetical prefab path for determinism across
        // runs (AssetDatabase.FindAssets does not guarantee a stable order).
        private static ComponentSnapshot PickMajorityRepresentative(
            List<(string PrefabPath, ComponentSnapshot Snap)> values)
        {
            // Sort by path first so that tie-breaking is stable and deterministic.
            var sorted = new List<(string PrefabPath, ComponentSnapshot Snap)>(values);
            sorted.Sort((a, b) => string.Compare(a.PrefabPath, b.PrefabPath, StringComparison.Ordinal));

            var bestSnap = sorted[0].Snap;
            var bestCount = 0;

            for (var i = 0; i < sorted.Count; i++)
            {
                var count = 0;
                for (var j = 0; j < sorted.Count; j++)
                {
                    if (FindDifferingProperties(sorted[j].Snap, sorted[i].Snap).Count == 0)
                        count++;
                }
                if (count > bestCount)
                {
                    bestCount = count;
                    bestSnap = sorted[i].Snap;
                }
            }
            return bestSnap;
        }

        private static string MakeKey(string path, string type) => path + "|" + type;
        private static string ExtractPath(string key) => key.Substring(0, key.LastIndexOf('|'));
        private static string ExtractType(string key) => key.Substring(key.LastIndexOf('|') + 1);
    }
}
