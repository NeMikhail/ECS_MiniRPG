using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Drop
{
    public readonly struct InventoryDropCommand
    {
        public readonly GameWorld.Entity _actor;
        public readonly InventoryDragData _sourceData;
        public readonly int _count;
        public readonly InventoryDropSourceType _sourceType;

        public InventoryDropCommand(GameWorld.Entity actor, InventoryDragData sourceData, int count, InventoryDropSourceType sourceType)
        {
            _actor = actor;
            _sourceData = sourceData;
            _count = count;
            _sourceType = sourceType;
        }
    }
}
