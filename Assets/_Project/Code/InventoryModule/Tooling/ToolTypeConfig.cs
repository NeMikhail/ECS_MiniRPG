using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Tooling
{
    [CreateAssetMenu(fileName = "ToolTypeConfig", menuName = "ECSMiniRPG/Inventory/Tool Type")]
    public sealed class ToolTypeConfig : ScriptableObject
    {
        [SerializeField] private string _id;

        public string Id => _id;
    }
}
