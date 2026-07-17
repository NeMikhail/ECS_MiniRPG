using ECSMiniRPG.LocationModule.Resources.Configs;
using ECSMiniRPG.LocationModule.Resources.Views;

namespace ECSMiniRPG.LocationModule.Resources
{
    public sealed class LocationResourceSpawnPointRuntimeData
    {
        private readonly LocationResourceSpawnPointView _view;
        private float _spawnTimer;
        private float _restoreTimer;
        private float _refreshTimer;
        private bool _hasActiveEntity;
        private GameWorld.Entity _activeEntity;
        private ResourceNodeConfig _currentResource;
        private bool _hasCurrentResourceSelection;

        public LocationResourceSpawnPointRuntimeData(LocationResourceSpawnPointView view)
        {
            _view = view;
            _spawnTimer = view.Config.InitialSpawnDelay;
            _restoreTimer = 0f;
            _refreshTimer = view.Config.ResourceRefreshInterval;
        }

        public LocationResourceSpawnPointView View => _view;
        public LocationResourceSpawnPointConfig Config => _view.Config;
        public float SpawnTimer => _spawnTimer;
        public float RestoreTimer => _restoreTimer;
        public float RefreshTimer => _refreshTimer;
        public bool HasActiveEntity => _hasActiveEntity;
        public GameWorld.Entity ActiveEntity => _activeEntity;
        public ResourceNodeConfig CurrentResource => _currentResource;
        public bool HasCurrentResourceSelection => _hasCurrentResourceSelection;

        public void SetSpawnTimer(float value)
        {
            _spawnTimer = value;
        }

        public void SetRestoreTimer(float value)
        {
            _restoreTimer = value;
        }

        public void ResetRefreshTimer()
        {
            _refreshTimer = Config.ResourceRefreshInterval;
        }

        public void SetRefreshTimer(float value)
        {
            _refreshTimer = value;
        }

        public void SetActiveEntity(GameWorld.Entity entity)
        {
            _activeEntity = entity;
            _hasActiveEntity = true;
        }

        public void ClearActiveEntity()
        {
            _activeEntity = default;
            _hasActiveEntity = false;
        }

        public void SetCurrentResource(ResourceNodeConfig resource)
        {
            _currentResource = resource;
            _hasCurrentResourceSelection = true;
        }
    }
}
