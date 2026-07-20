using System.Collections.Generic;
using ECSMiniRPG.MobModule.Configs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobSpawnPoolRadiusFileName, menuName = ConfigAssetNames._mobSpawnPoolRadiusMenuName)]
    public sealed class Spawn_PoolRadiusBehaviourConfig : MobSpawnBehaviourConfig
    {
        [SerializeField] private MobSpawnSelectionMode _mobSelectionMode = MobSpawnSelectionMode.Single;
        [SerializeField] private MobConfig _mobConfig;
        [SerializeField] private List<MobConfig> _mobConfigs = new List<MobConfig>();
        [SerializeField] private int _maxCount = 5;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private float _radius = 5f;

        public MobSpawnSelectionMode MobSelectionMode => _mobSelectionMode;
        public MobConfig MobConfig => _mobConfig;
        public IReadOnlyList<MobConfig> MobConfigs => _mobConfigs;
        public int MaxCount => _maxCount;
        public float SpawnInterval => _spawnInterval;
        public float Radius => _radius;

        public override void Execute(MobSpawnContext context)
        {
            var mobConfig = GetMobConfig();

            if (context._spawner.SpawnedMobs.Count < _maxCount && context._time >= context._spawner.NextSpawnTime && CanSpawn(mobConfig))
            {
                context._spawner.NextSpawnTime = context._time + _spawnInterval;
                context._commandQueue.Enqueue(mobConfig, context._spawner.GetRandomPointInRadius(_radius), context._spawner.transform.rotation, context._spawner);
            }
        }

        private MobConfig GetMobConfig()
        {
            var mobConfig = _mobConfig;

            if (_mobSelectionMode == MobSpawnSelectionMode.List && _mobConfigs.Count != 0)
            {
                mobConfig = _mobConfigs[Random.Range(0, _mobConfigs.Count)];
            }

            return mobConfig;
        }

        private bool CanSpawn(MobConfig mobConfig)
        {
            return mobConfig != null && mobConfig.HasPrefabKey;
        }
    }
}


