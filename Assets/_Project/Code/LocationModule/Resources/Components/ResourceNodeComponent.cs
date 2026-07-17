using ECSMiniRPG.LocationModule.Resources.Configs;
using ECSMiniRPG.LocationModule.Resources.Views;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.LocationModule.Resources.Components
{
    public struct ResourceNodeComponent : IComponent
    {
        public ResourceNodeConfig _config;
        public LocationResourceSpawnPointRuntimeData _spawnPoint;
        public ResourceNodeView _view;
        public LocationResourceNodeState _state;
    }
}
