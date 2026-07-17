using ECSMiniRPG.InventoryModule.Items;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Tooling
{
    [CreateAssetMenu(fileName = "ToolItemConfig", menuName = "ECSMiniRPG/Inventory/Tool Item")]
    public sealed class ToolItemConfig : ItemPreset
    {
        [SerializeField] private ToolTypeConfig _toolType;

        public ToolTypeConfig ToolType => _toolType;
    }
}
