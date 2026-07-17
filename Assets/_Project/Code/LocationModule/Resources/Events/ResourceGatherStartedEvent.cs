using ECSMiniRPG.LocationModule.Resources.Configs;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.LocationModule.Resources.Events
{
    public struct ResourceGatherStartedEvent : IEvent
    {
        public GameWorld.Entity _actor;
        public GameWorld.Entity _target;
        public ResourceNodeConfig _resource;
        public float _duration;
        public float _range;
    }
}
