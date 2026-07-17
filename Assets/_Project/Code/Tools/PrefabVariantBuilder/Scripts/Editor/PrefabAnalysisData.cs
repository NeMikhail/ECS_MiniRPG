using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MAEngine.PrefabVariantBuilder
{
    internal class PropertySnapshot
    {
        internal string PropertyPath;
        internal SerializedPropertyType Type;
        internal string StringValue;
        internal float  FloatValue;
        internal bool   BoolValue;
        internal Object ObjectRefValue;
        internal string VectorValue; // "x,y,z,w" for Color/Vector/Quaternion
    }

    internal class ComponentSnapshot
    {
        internal string TypeAssemblyQualified;
        internal string TypeDisplayName;
        internal List<PropertySnapshot> Properties = new List<PropertySnapshot>();
    }

    internal class HierarchyNode
    {
        internal string TransformPath; // "" = root, "Body/LeftArm" = child
        internal List<ComponentSnapshot> Components = new List<ComponentSnapshot>();
        internal List<HierarchyNode> Children = new List<HierarchyNode>();
    }

    internal class PrefabData
    {
        internal string AssetPath;
        internal string PrefabName;
        internal HierarchyNode Root = new HierarchyNode();
    }

    internal class SharedEntry
    {
        internal string TransformPath;
        internal ComponentSnapshot Representative; // first-wins values for root
    }

    internal class VariantOverride
    {
        internal string TransformPath;
        internal string TypeAssemblyQualified;
        internal List<PropertySnapshot> DifferingProps = new List<PropertySnapshot>();
    }

    internal class AnalysisResult
    {
        internal List<PrefabData> AllPrefabs = new List<PrefabData>();
        internal List<SharedEntry> SharedEntries = new List<SharedEntry>();
        internal Dictionary<string, List<VariantOverride>> PrefabOverrides
            = new Dictionary<string, List<VariantOverride>>();
        internal Dictionary<string, List<(string Path, ComponentSnapshot Comp)>> UniqueComponents
            = new Dictionary<string, List<(string, ComponentSnapshot)>>();
        internal Dictionary<string, List<HierarchyNode>> UniquePaths
            = new Dictionary<string, List<HierarchyNode>>();
        internal Dictionary<string, List<(string Path, string TypeName)>> RemovedEntries
            = new Dictionary<string, List<(string, string)>>();
    }
}
