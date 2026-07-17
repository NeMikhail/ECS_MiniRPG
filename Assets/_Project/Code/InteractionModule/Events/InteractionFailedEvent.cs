using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InteractionModule.Events
{
    public struct InteractionFailedEvent : IEvent
    {
        public GameWorld.Entity _actor;
        public GameWorld.Entity _target;
        public InteractionFailReason _reason;
    }
}
