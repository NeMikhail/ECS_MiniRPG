using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.Events
{
    public struct HealedEvent : IEvent
    {
        public GameWorld.Entity _target;
        public float _requestedHeal;
        public float _appliedHeal;
    }
}