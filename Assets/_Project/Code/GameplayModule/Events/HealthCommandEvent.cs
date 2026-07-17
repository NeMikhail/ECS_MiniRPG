using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.Events
{
    public struct HealthCommandEvent : IEvent
    {
        public HealthCommandType _type;
        public GameWorld.Entity _target;
        public float _value;
    }
}