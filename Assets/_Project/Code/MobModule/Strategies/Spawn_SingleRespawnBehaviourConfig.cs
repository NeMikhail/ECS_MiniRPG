using ECSMiniRPG.MobModule.Configs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobSpawnSingleRespawnFileName, menuName = ConfigAssetNames._mobSpawnSingleRespawnMenuName)]
    public sealed class Spawn_SingleRespawnBehaviourConfig : MobSpawnBehaviourConfig
    {
        [SerializeField] private MobConfig _mobConfig;
        [SerializeField] private float _respawnDelay = 5f;

        public MobConfig MobConfig => _mobConfig;
        public float RespawnDelay => _respawnDelay;

        public override void Execute(MobSpawnContext context)
        {
            if (context._spawner.SpawnedMobs.Count == 0 && context._time >= context._spawner.NextSpawnTime && CanSpawn(_mobConfig))
            {
                context._spawner.NextSpawnTime = context._time + _respawnDelay;
                context._commandQueue.Enqueue(_mobConfig, context._spawner.transform.position, context._spawner.transform.rotation, context._spawner);
            }
        }

        private bool CanSpawn(MobConfig mobConfig)
        {
            return mobConfig != null && mobConfig.HasPrefabKey;
        }
    }
}


