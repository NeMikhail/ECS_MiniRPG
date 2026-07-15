using ECSMiniRPG.Core;
using UnityEngine;

namespace ECSMiniRPG.LocationModule
{
    public sealed class LocationModuleFactory : IEcsModuleFactory
    {
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
        }

        public void RegisterUpdateSystems()
        {
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }

        private void SetupLocationView()
        {
            var locationView = _viewsProvider.LocationView;

            if (locationView == null)
            {
                Debug.LogError("LocationView is not assigned in ViewsProvider.");
            }

            if (locationView != null)
            {
                _locationRuntimeData.SetLocationView(locationView);
            }
        }
    }
}
