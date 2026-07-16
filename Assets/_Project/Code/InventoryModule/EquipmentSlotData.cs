using System;

namespace ECSMiniRPG.InventoryModule
{
    [Serializable]
    public sealed class EquipmentSlotData
    {
        private EquipmentSlotId _slotId;
        private EquipmentSlotType _slotType;
        private InventoryItemInstance _item;

        public EquipmentSlotData(EquipmentSlotId slotId, EquipmentSlotType slotType)
        {
            _slotId = slotId;
            _slotType = slotType;
        }

        public EquipmentSlotId SlotId => _slotId;
        public EquipmentSlotType SlotType => _slotType;
        public InventoryItemInstance Item => _item;
        public bool HasItem => _item != null;

        public void SetItem(InventoryItemInstance item)
        {
            _item = item;
        }

        public void Clear()
        {
            _item = null;
        }
    }
}
