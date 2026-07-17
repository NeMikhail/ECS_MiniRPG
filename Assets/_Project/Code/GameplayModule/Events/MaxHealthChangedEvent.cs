using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.Events
{
    public struct MaxHealthChangedEvent : IEvent
    {
        public GameWorld.Entity _target;
        public float _previousValue;
        public float _currentValue;
    }
}