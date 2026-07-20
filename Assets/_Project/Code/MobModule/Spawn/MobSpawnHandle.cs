using ECSMiniRPG.MobModule.Configs;
using ECSMiniRPG.MobModule.Views;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.MobModule.Spawn
{
    public readonly struct MobSpawnHandle
    {
        public readonly MobConfig _config;
        public readonly MobSpawnerView _spawner;
        public readonly AsyncOperationHandle<GameObject> _handle;

        public MobSpawnHandle(MobConfig config, MobSpawnerView spawner, AsyncOperationHandle<GameObject> handle)
        {
            _config = config;
            _spawner = spawner;
            _handle = handle;
        }
    }
}
