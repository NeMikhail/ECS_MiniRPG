using ECSMiniRPG.Core;
using ECSMiniRPG.MobModule.Spawn;
using ECSMiniRPG.MobModule.Systems;

namespace ECSMiniRPG.MobModule
{
    public sealed class MobModuleFactory : IEcsModuleFactory
    {
        private static readonly short _spawnerSystemOrder = 10;
        private static readonly short _spawnSystemOrder = 20;
        private static readonly short _targetSearchSystemOrder = 30;
        private static readonly short _movementSystemOrder = 35;
        private static readonly short _attackSystemOrder = 39;

        public void RegisterResources()
        {
            GameWorld.SetResource(new MobSpawnCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new MobSpawnerSystem(), _spawnerSystemOrder);
            GameSystems.Add(new MobSpawnSystem(), _spawnSystemOrder);
            GameSystems.Add(new MobTargetSearchSystem(), _targetSearchSystemOrder);
            GameSystems.Add(new MobMovementSystem(), _movementSystemOrder);
            GameSystems.Add(new MobAttackSystem(), _attackSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }
    }
}

