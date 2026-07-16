namespace ECSMiniRPG.InventoryModule
{
    public enum InventoryCommandType
    {
        None = 0,
        UseOrEquipInventorySlot = 1,
        UnequipEquipmentSlot = 2,
        MoveInventorySlot = 3,
        MoveInventoryToEquipment = 4,
        MoveEquipmentToInventory = 5,
        MoveEquipmentSlot = 6
    }
}
