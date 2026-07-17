using ECSMiniRPG.InventoryModule.Items;
using ECSMiniRPG.InventoryModule.Pickup;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Events
{
    public struct InventoryPickupEvent : IEvent
    {
        public GameWorld.Entity _actor;
        public GameWorld.Entity _target;
        public ItemPreset _item;
        public int _requestedCount;
        public int _addedCount;
        public int _remainingCount;
        public InventoryPickupResultType _resultType;
        public InventoryPickupSourceType _sourceType;
    }
}
