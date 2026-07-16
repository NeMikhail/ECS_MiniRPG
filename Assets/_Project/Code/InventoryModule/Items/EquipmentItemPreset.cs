using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Items
{
    [CreateAssetMenu(fileName = "EquipmentItemPreset", menuName = "ECSMiniRPG/Inventory/Equipment Item")]
    public class EquipmentItemPreset : ItemPreset, IEquipmentItem
    {
        [SerializeField] private EquipmentSlotType _slotType = EquipmentSlotType.OffHand;

        public EquipmentSlotType SlotType => _slotType;
    }
}
