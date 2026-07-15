using ECSMiniRPG.ContentManagement;

namespace ECSMiniRPG.Core
{
    public sealed class CoreModuleFactory : IEcsModuleFactory
    {
        private readonly GameConfigs _gameConfigs;
        private readonly ContentManagementSystem _contentManagementSystem;

        public CoreModuleFactory(GameConfigs gameConfigs, ContentManagementSystem contentManagementSystem)
        {
            _gameConfigs = gameConfigs;
            _contentManagementSystem = contentManagementSystem;
        }

        public void RegisterResources()
        {
            GameWorld.SetResource(_contentManagementSystem);
            GameWorld.SetResource(_gameConfigs);
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
    }
}
