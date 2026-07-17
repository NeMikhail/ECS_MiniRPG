using ECSMiniRPG.Core;
using ECSMiniRPG.LocationModule.Resources.Loot;
using ECSMiniRPG.LocationModule.Resources.Systems;

namespace ECSMiniRPG.LocationModule
{
    public sealed class LocationModuleFactory : IEcsModuleFactory
    {
        private static readonly short _resourceInteractionFailureSystemOrder = 10;
        private static readonly short _resourceSpawnSystemOrder = 20;
        private static readonly short _lootSpawnSystemOrder = 30;

        private readonly ViewsProvider _viewsProvider;
        private LocationRuntimeData _locationRuntimeData;

        public LocationModuleFactory(ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
        }

        public void RegisterResources()
        {
            _locationRuntimeData = new LocationRuntimeData();
            SetupLocationView();
            GameWorld.SetResource(_locationRuntimeData);
            GameWorld.SetResource(new LocationLootSpawnCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new ResourceGatherInteractionFailureSystem(), _resourceInteractionFailureSystemOrder);
            GameSystems.Add(new LocationResourceSpawnSystem(), _resourceSpawnSystemOrder);
            GameSystems.Add(new LocationLootSpawnSystem(), _lootSpawnSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }

        private void SetupLocationView()
        {
            var locationView = _viewsProvider.GetView<LocationView>();

            if (locationView != null)
            {
                _locationRuntimeData.SetLocationView(locationView);
            }
        }
    }
}
