using ECSMiniRPG.InventoryModule.Tooling;
using ECSMiniRPG.LocationModule.Resources.Configs;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.LocationModule.Resources.Events
{
    public struct ResourceGatherFailedEvent : IEvent
    {
        public GameWorld.Entity _actor;
        public GameWorld.Entity _target;
        public ResourceNodeConfig _resource;
        public ToolTypeConfig _requiredTool;
        public ResourceGatherFailReason _reason;
    }
}
