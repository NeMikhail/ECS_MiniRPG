using ECSMiniRPG.MobModule.Spawn;
using ECSMiniRPG.MobModule.Views;

namespace ECSMiniRPG.MobModule.Strategies
{
    public readonly struct MobSpawnContext
    {
        public readonly MobSpawnerView _spawner;
        public readonly MobSpawnCommandQueue _commandQueue;
        public readonly float _time;

        public MobSpawnContext(MobSpawnerView spawner, MobSpawnCommandQueue commandQueue, float time)
        {
            _spawner = spawner;
            _commandQueue = commandQueue;
            _time = time;
        }
    }
}
