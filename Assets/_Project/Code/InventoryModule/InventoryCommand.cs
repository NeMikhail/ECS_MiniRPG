namespace ECSMiniRPG.InventoryModule
{
    public readonly struct InventoryCommand
    {
        public readonly InventoryCommandType _type;
        public readonly int _sourceInventoryIndex;
        public readonly int _targetInventoryIndex;
        public readonly EquipmentSlotId _sourceEquipmentSlotId;
        public readonly EquipmentSlotId _targetEquipmentSlotId;

        public InventoryCommand(
            InventoryCommandType type,
            int sourceInventoryIndex,
            int targetInventoryIndex,
            EquipmentSlotId sourceEquipmentSlotId,
            EquipmentSlotId targetEquipmentSlotId
        )
        {
            _type = type;
            _sourceInventoryIndex = sourceInventoryIndex;
            _targetInventoryIndex = targetInventoryIndex;
            _sourceEquipmentSlotId = sourceEquipmentSlotId;
            _targetEquipmentSlotId = targetEquipmentSlotId;
        }

        public static InventoryCommand UseOrEquipInventorySlot(int slotIndex)
        {
            return new InventoryCommand(InventoryCommandType.UseOrEquipInventorySlot, slotIndex, -1, EquipmentSlotId.None, EquipmentSlotId.None);
        }

        public static InventoryCommand UnequipEquipmentSlot(EquipmentSlotId slotId)
        {
            return new InventoryCommand(InventoryCommandType.UnequipEquipmentSlot, -1, -1, slotId, EquipmentSlotId.None);
        }

        public static InventoryCommand MoveInventorySlot(int sourceIndex, int targetIndex)
        {
            return new InventoryCommand(InventoryCommandType.MoveInventorySlot, sourceIndex, targetIndex, EquipmentSlotId.None, EquipmentSlotId.None);
        }

        public static InventoryCommand MoveInventoryToEquipment(int sourceIndex, EquipmentSlotId targetSlotId)
        {
            return new InventoryCommand(InventoryCommandType.MoveInventoryToEquipment, sourceIndex, -1, EquipmentSlotId.None, targetSlotId);
        }

        public static InventoryCommand MoveEquipmentToInventory(EquipmentSlotId sourceSlotId, int targetIndex)
        {
            return new InventoryCommand(InventoryCommandType.MoveEquipmentToInventory, -1, targetIndex, sourceSlotId, EquipmentSlotId.None);
        }

        public static InventoryCommand MoveEquipmentSlot(EquipmentSlotId sourceSlotId, EquipmentSlotId targetSlotId)
        {
            return new InventoryCommand(InventoryCommandType.MoveEquipmentSlot, -1, -1, sourceSlotId, targetSlotId);
        }
    }
}
