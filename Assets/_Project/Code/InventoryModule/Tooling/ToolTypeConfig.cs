using System.Collections.Generic;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Tooling
{
    [CreateAssetMenu(fileName = "ToolTypeConfig", menuName = "ECSMiniRPG/Inventory/Tool Type")]
    public sealed class ToolTypeConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private List<ToolTypeConfig> _includedToolTypes = new List<ToolTypeConfig>();

        public string Id => _id;
        public IReadOnlyList<ToolTypeConfig> IncludedToolTypes => _includedToolTypes;
    }
}
