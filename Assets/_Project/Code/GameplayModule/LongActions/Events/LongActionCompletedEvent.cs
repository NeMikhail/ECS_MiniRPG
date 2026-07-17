using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.LongActions.Events
{
    public struct LongActionCompletedEvent : IEvent
    {
        public GameWorld.Entity _owner;
        public GameWorld.Entity _target;
    }
}
