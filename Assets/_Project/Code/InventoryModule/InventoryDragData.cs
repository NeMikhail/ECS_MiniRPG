namespace ECSMiniRPG.InventoryModule
{
    public readonly struct InventoryDragData
    {
        public readonly InventorySlotSourceType _sourceType;
        public readonly int _inventoryIndex;
        public readonly EquipmentSlotId _equipmentSlotId;

        public InventoryDragData(InventorySlotSourceType sourceType, int inventoryIndex, EquipmentSlotId equipmentSlotId)
        {
            _sourceType = sourceType;
            _inventoryIndex = inventoryIndex;
            _equipmentSlotId = equipmentSlotId;
        }

        public static InventoryDragData Inventory(int inventoryIndex)
        {
            return new InventoryDragData(InventorySlotSourceType.Inventory, inventoryIndex, EquipmentSlotId.None);
        }

        public static InventoryDragData Equipment(EquipmentSlotId equipmentSlotId)
        {
            return new InventoryDragData(InventorySlotSourceType.Equipment, -1, equipmentSlotId);
        }
    }
}
