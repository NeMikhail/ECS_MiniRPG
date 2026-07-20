using ECSMiniRPG.MobModule.Configs;
using ECSMiniRPG.MobModule.Views;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Spawn
{
    public readonly struct MobSpawnCommand
    {
        public readonly MobConfig _config;
        public readonly Vector3 _position;
        public readonly Quaternion _rotation;
        public readonly MobSpawnerView _spawner;

        public MobSpawnCommand(MobConfig config, Vector3 position, Quaternion rotation, MobSpawnerView spawner)
        {
            _config = config;
            _position = position;
            _rotation = rotation;
            _spawner = spawner;
        }
    }
}
