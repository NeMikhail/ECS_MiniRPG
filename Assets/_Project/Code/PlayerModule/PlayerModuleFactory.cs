using ECSMiniRPG.Core;
using ECSMiniRPG.PlayerModule.Configs;
using ECSMiniRPG.PlayerModule.Systems;

namespace ECSMiniRPG.PlayerModule
{
    public sealed class PlayerModuleFactory : IEcsModuleFactory
    {
        private static readonly short _spawnSystemOrder = 0;
        private static readonly short _movementSystemOrder = 0;

        private readonly GameConfigs _gameConfigs;

        public PlayerModuleFactory(GameConfigs gameConfigs)
        {
            _gameConfigs = gameConfigs;
        }

        public void RegisterResources()
        {
            GameWorld.SetResource(new PlayerSpawnRuntimeData(_gameConfigs.PlayerConfig.InitialSpawnStrategy));
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new PlayerSpawnSystem(), _spawnSystemOrder);
        }

        public void RegisterFixedSystems()
        {
            GameFixedSystems.Add(new PlayerMovementSystem(), _movementSystemOrder);
        }

        public void Destroy()
        {
        }
    }
}
