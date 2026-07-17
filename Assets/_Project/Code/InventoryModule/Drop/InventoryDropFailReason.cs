namespace ECSMiniRPG.InventoryModule.Drop
{
    public enum InventoryDropFailReason
    {
        None = 0,
        InvalidSource = 1,
        EmptySlot = 2,
        MissingLootConfig = 3,
        MissingActorTransform = 4
    }
}
