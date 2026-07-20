using System.Collections.Generic;
using ECSMiniRPG.MobModule.Configs;
using ECSMiniRPG.MobModule.Views;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Spawn
{
    public sealed class MobSpawnCommandQueue : IResource
    {
        private readonly Queue<MobSpawnCommand> _commands = new Queue<MobSpawnCommand>();

        public int Count => _commands.Count;

        public void Enqueue(MobConfig config, Vector3 position, Quaternion rotation)
        {
            Enqueue(config, position, rotation, null);
        }

        public void Enqueue(MobConfig config, Vector3 position, Quaternion rotation, MobSpawnerView spawner)
        {
            _commands.Enqueue(new MobSpawnCommand(config, position, rotation, spawner));
        }

        public MobSpawnCommand Dequeue()
        {
            return _commands.Dequeue();
        }
    }
}
