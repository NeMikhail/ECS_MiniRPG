using ECSMiniRPG.Core;

namespace ECSMiniRPG.LoggerModule
{
    public sealed class LoggerModuleFactory : IEcsModuleFactory
    {
        private readonly GameConfigs _gameConfigs;

        public LoggerModuleFactory(GameConfigs gameConfigs)
        {
            _gameConfigs = gameConfigs;
        }

        public void RegisterResources()
        {
            LoggerSystem.ApplyConfig(_gameConfigs.LoggerModuleConfig);
        }

        public void RegisterUpdateSystems()
        {
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
            LoggerSystem.Clear();
        }
    }
}