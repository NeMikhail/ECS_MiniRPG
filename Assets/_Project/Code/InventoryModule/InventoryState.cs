using System.Collections.Generic;

namespace ECSMiniRPG.InventoryModule
{
    public sealed class InventoryState
    {
        private readonly List<InventorySlotData> _inventorySlots = new List<InventorySlotData>();
        private readonly List<EquipmentSlotData> _equipmentSlots = new List<EquipmentSlotData>();
        private long _nextInstanceId = 1;

        public List<InventorySlotData> InventorySlots => _inventorySlots;
        public List<EquipmentSlotData> EquipmentSlots => _equipmentSlots;

        public void Clear()
        {
            _inventorySlots.Clear();
            _equipmentSlots.Clear();
            _nextInstanceId = 1;
        }

        public long GetNextInstanceId()
        {
            var instanceId = _nextInstanceId;
            _nextInstanceId++;
            return instanceId;
        }
    }
}
