using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.LongActions.Events
{
    public struct LongActionInterruptEvent : IEvent
    {
        public GameWorld.Entity _owner;
        public LongActionCancelReason _reason;
    }
}
