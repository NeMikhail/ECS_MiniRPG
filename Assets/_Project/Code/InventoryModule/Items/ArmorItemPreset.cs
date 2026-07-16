using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Items
{
    [CreateAssetMenu(fileName = "ArmorItemPreset", menuName = "ECSMiniRPG/Inventory/Armor Item")]
    public sealed class ArmorItemPreset : EquipmentItemPreset, IArmor
    {
        [SerializeField] private float _armor = 1f;

        public float Armor => _armor;
    }
}
