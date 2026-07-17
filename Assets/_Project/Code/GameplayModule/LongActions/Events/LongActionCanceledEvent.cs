using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.LongActions.Events
{
    public struct LongActionCanceledEvent : IEvent
    {
        public GameWorld.Entity _owner;
        public GameWorld.Entity _target;
        public LongActionCancelReason _reason;
    }
}
