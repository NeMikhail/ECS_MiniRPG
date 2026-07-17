using UnityEditor;
using UnityEngine;

namespace MAEngine.PrefabVariantBuilder
{
    internal static class PrefabHierarchyTraverser
    {
        internal static HierarchyNode Traverse(GameObject prefabRoot)
        {
            var node = TraverseNode(prefabRoot.transform, string.Empty);
            return node;
        }

        private static HierarchyNode TraverseNode(Transform t, string path)
        {
            var node = new HierarchyNode { TransformPath = path };

            // Capture GameObject-level properties (m_Layer, m_TagString, etc.) as a special snapshot.
            // These are NOT exposed via GetComponents<Component>() — they live on the GameObject itself.
            node.Components.Add(SnapshotGameObject(t.gameObject));

            foreach (var comp in t.GetComponents<Component>())
            {
                if (comp == null) continue;

                // Transform cannot be added via AddComponent — capture only the layout properties
                // we care about via a dedicated sentinel snapshot (same pattern as GameObject).
                if (comp is Transform tr)
                {
                    node.Components.Add(SnapshotTransform(tr));
                    continue;
                }

                var snapshot = SnapshotComponent(comp);
                node.Components.Add(snapshot);
            }

            foreach (Transform child in t)
            {
                var childPath = string.IsNullOrEmpty(path) ? child.name : path + "/" + child.name;
                node.Children.Add(TraverseNode(child, childPath));
            }

            return node;
        }

        private static ComponentSnapshot SnapshotComponent(Component comp)
        {
            var snapshot = new ComponentSnapshot
            {
                TypeAssemblyQualified = comp.GetType().AssemblyQualifiedName,
                TypeDisplayName = comp.GetType().Name
            };

            var so = new SerializedObject(comp);
            var prop = so.GetIterator();
            // Use Next (not NextVisible) to include hidden properties such as m_Enabled on built-in components,
            // which NextVisible skips because the Inspector renders them via the component header, not a field drawer.
            if (!prop.Next(true)) return snapshot;

            do
            {
                // Skip Unity-internal meta-properties that must never be overridden on new component instances:
                //   m_Script                   — MonoBehaviour type reference (not a user property)
                //   m_ObjectHideFlags          — Unity internal hide-flags
                //   m_CorrespondingSourceObject — prefab-variant source linkage
                //   m_PrefabInstance           — prefab instance tracking
                //   m_PrefabAsset              — prefab asset reference
                //   m_GameObject               — per-instance component-to-owner link; each component is
                //                                owned by a different GO, so this differs across prefabs and
                //                                applying it to a variant corrupts the component reference,
                //                                causing the "broken GameObject reference" error.
                switch (prop.name)
                {
                    case "m_Script":
                    case "m_ObjectHideFlags":
                    case "m_CorrespondingSourceObject":
                    case "m_PrefabInstance":
                    case "m_PrefabAsset":
                    case "m_GameObject":
                        continue;
                }

                if (prop.propertyType == SerializedPropertyType.Generic ||
                    prop.propertyType == SerializedPropertyType.ManagedReference ||
                    prop.propertyType == SerializedPropertyType.AnimationCurve ||
                    prop.propertyType == SerializedPropertyType.Gradient)
                {
                    // Recursively capture leaf sub-properties (e.g. m_Materials.Array.data[0],
                    // m_IncludeLayers.m_Bits). The full propertyPath stored in each PropertySnapshot
                    // is used by ApplySnapshot / ApplyOverrides via so.FindProperty(path).
                    // AnimationCurve and Gradient are routed here too: their keys/stops are structs
                    // of floats/Colors that CaptureGenericChildren handles natively.
                    // Known limitation: if two prefabs have AnimationCurves with a different number
                    // of keyframes, FindDifferingProperties will only detect value differences for
                    // keys that exist in the representative; structural differences (added/removed
                    // keys) are not detected.
                    CaptureGenericChildren(prop.Copy(), snapshot);
                }
                else
                {
                    snapshot.Properties.Add(PropertyValueResolver.Capture(prop.Copy()));
                }
            }
            while (prop.Next(false));

            return snapshot;
        }

        // Captures the small set of serialized properties that belong to the GameObject record
        // itself (layer, tag, active state, static flags, nav-mesh layer). These are not exposed
        // via GetComponents<Component>() and must be read via SerializedObject(go) directly.
        // A sentinel type (typeof(GameObject)) lets the downstream builders distinguish this
        // snapshot from ordinary component snapshots without extra plumbing.
        private static ComponentSnapshot SnapshotGameObject(GameObject go)
        {
            var snapshot = new ComponentSnapshot
            {
                TypeAssemblyQualified = typeof(GameObject).AssemblyQualifiedName,
                TypeDisplayName       = "GameObject"
            };

            var so = new SerializedObject(go);
            foreach (var propName in new[] { "m_Layer", "m_TagString", "m_IsActive", "m_StaticEditorFlags", "m_NavMeshLayer" })
            {
                var prop = so.FindProperty(propName);
                if (prop != null)
                    snapshot.Properties.Add(PropertyValueResolver.Capture(prop.Copy()));
            }

            return snapshot;
        }

        // Captures the layout properties of a Transform (position, rotation, scale).
        // Transform always exists on a GameObject and cannot be added via AddComponent, so it is
        // handled as a sentinel — downstream builders apply properties directly to the existing
        // Transform rather than trying to create a new one.
        private static ComponentSnapshot SnapshotTransform(Transform t)
        {
            var snapshot = new ComponentSnapshot
            {
                TypeAssemblyQualified = typeof(Transform).AssemblyQualifiedName,
                TypeDisplayName       = "Transform"
            };

            var so = new SerializedObject(t);
            // m_LocalPosition and m_LocalScale are Vector3; m_LocalRotation is Quaternion.
            // These are leaf types in PropertyValueResolver — use Capture directly, not CaptureGenericChildren.
            foreach (var propName in new[] { "m_LocalPosition", "m_LocalRotation", "m_LocalScale", "m_LocalEulerAnglesHint" })
            {
                var prop = so.FindProperty(propName);
                if (prop != null)
                    snapshot.Properties.Add(PropertyValueResolver.Capture(prop.Copy()));
            }
            return snapshot;
        }

        // Recursively enters a Generic or ManagedReference property and captures all leaf
        // sub-properties (those whose type is NOT Generic / ManagedReference) by their full
        // propertyPath. Uses GetEndProperty() as the loop sentinel so that the iterator
        // correctly terminates after traversing the entire subtree regardless of nesting depth.
        private static void CaptureGenericChildren(SerializedProperty parent, ComponentSnapshot snapshot)
        {
            if (!parent.hasChildren) return;

            var end   = parent.GetEndProperty();
            var child = parent.Copy();
            if (!child.Next(true)) return; // enter first child

            while (!SerializedProperty.EqualContents(child, end))
            {
                if (child.propertyType == SerializedPropertyType.Generic ||
                    child.propertyType == SerializedPropertyType.ManagedReference)
                {
                    CaptureGenericChildren(child.Copy(), snapshot); // recurse
                }
                else
                {
                    snapshot.Properties.Add(PropertyValueResolver.Capture(child.Copy()));
                }

                if (!child.Next(false)) break;
            }
        }
    }
}
