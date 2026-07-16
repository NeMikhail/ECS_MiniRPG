namespace ECSMiniRPG.InventoryModule.Items
{
    public interface IEquipmentItem : IItem
    {
        EquipmentSlotType SlotType { get; }
    }
}
