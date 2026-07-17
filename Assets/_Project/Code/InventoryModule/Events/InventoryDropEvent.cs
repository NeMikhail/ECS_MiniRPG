using ECSMiniRPG.InventoryModule.Drop;
using ECSMiniRPG.InventoryModule.Items;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Events
{
    public struct InventoryDropEvent : IEvent
    {
        public GameWorld.Entity _actor;
        public ItemPreset _item;
        public InventoryDragData _sourceData;
        public int _requestedCount;
        public int _droppedCount;
        public InventoryDropResultType _resultType;
        public InventoryDropSourceType _sourceType;
        public InventoryDropFailReason _failReason;
    }
}
