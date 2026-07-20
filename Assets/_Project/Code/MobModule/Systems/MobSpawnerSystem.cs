using ECSMiniRPG.MobModule.Spawn;
using ECSMiniRPG.MobModule.Strategies;
using ECSMiniRPG.MobModule.Views;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Systems
{
    public struct MobSpawnerSystem : ISystem
    {
        public void Update()
        {
            ref var commandQueue = ref GameWorld.GetResource<MobSpawnCommandQueue>();

            for (var i = 0; i < MobSpawnerView.Spawners.Count; i++)
            {
                ExecuteSpawner(MobSpawnerView.Spawners[i], commandQueue);
            }
        }

        private void ExecuteSpawner(MobSpawnerView spawner, MobSpawnCommandQueue commandQueue)
        {
            spawner.RemoveDestroyedMobs();

            if (spawner.SpawnBehaviour != null)
            {
                var context = new MobSpawnContext(spawner, commandQueue, Time.time);
                spawner.SpawnBehaviour.Execute(context);
            }
        }
    }
}
