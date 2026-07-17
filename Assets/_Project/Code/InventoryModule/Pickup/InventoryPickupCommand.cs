namespace ECSMiniRPG.InventoryModule.Pickup
{
    public readonly struct InventoryPickupCommand
    {
        public readonly GameWorld.Entity _actor;
        public readonly GameWorld.Entity _target;
        public readonly InventoryPickupSourceType _sourceType;

        public InventoryPickupCommand(GameWorld.Entity actor, GameWorld.Entity target, InventoryPickupSourceType sourceType)
        {
            _actor = actor;
            _target = target;
            _sourceType = sourceType;
        }
    }
}
