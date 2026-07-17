using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.Events
{
    public struct DamageTakenEvent : IEvent
    {
        public GameWorld.Entity _target;
        public float _requestedDamage;
        public float _appliedDamage;
    }
}