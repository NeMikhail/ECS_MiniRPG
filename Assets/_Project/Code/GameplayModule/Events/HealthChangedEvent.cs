using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.Events
{
    public struct HealthChangedEvent : IEvent
    {
        public GameWorld.Entity _target;
        public float _previousValue;
        public float _currentValue;
        public float _maxValue;
    }
}