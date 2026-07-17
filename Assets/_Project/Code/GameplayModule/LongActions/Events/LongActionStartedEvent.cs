using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.LongActions.Events
{
    public struct LongActionStartedEvent : IEvent
    {
        public GameWorld.Entity _owner;
        public GameWorld.Entity _target;
        public float _duration;
    }
}
