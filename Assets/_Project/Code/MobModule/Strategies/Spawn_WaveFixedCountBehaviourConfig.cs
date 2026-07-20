using System.Collections.Generic;
using ECSMiniRPG.MobModule.Configs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobSpawnWaveFixedCountFileName, menuName = ConfigAssetNames._mobSpawnWaveFixedCountMenuName)]
    public sealed class Spawn_WaveFixedCountBehaviourConfig : MobSpawnBehaviourConfig
    {
        [SerializeField] private MobSpawnSelectionMode _mobSelectionMode = MobSpawnSelectionMode.Single;
        [SerializeField] private MobConfig _mobConfig;
        [SerializeField] private List<MobConfig> _mobConfigs = new List<MobConfig>();
        [SerializeField] private int _mobCountPerWave = 3;
        [SerializeField] private int _wavesCount = 3;
        [SerializeField] private float _waveInterval = 10f;
        [SerializeField] private float _radius = 5f;

        public MobSpawnSelectionMode MobSelectionMode => _mobSelectionMode;
        public MobConfig MobConfig => _mobConfig;
        public IReadOnlyList<MobConfig> MobConfigs => _mobConfigs;
        public int MobCountPerWave => _mobCountPerWave;
        public int WavesCount => _wavesCount;
        public float WaveInterval => _waveInterval;
        public float Radius => _radius;

        public override void Execute(MobSpawnContext context)
        {
            if (context._spawner.WaveIndex < _wavesCount && context._time >= context._spawner.NextSpawnTime && HasMobConfig())
            {
                context._spawner.WaveIndex++;
                context._spawner.NextSpawnTime = context._time + _waveInterval;
                EnqueueWave(context);
            }
        }

        private void EnqueueWave(MobSpawnContext context)
        {
            for (var i = 0; i < _mobCountPerWave; i++)
            {
                var mobConfig = GetMobConfig(i);

                if (CanSpawn(mobConfig))
                {
                    context._commandQueue.Enqueue(mobConfig, context._spawner.GetRandomPointInRadius(_radius), context._spawner.transform.rotation, context._spawner);
                }
            }
        }

        private bool HasMobConfig()
        {
            var hasMobConfig = _mobConfig != null;

            if (_mobSelectionMode == MobSpawnSelectionMode.List)
            {
                hasMobConfig = _mobConfigs.Count != 0;
            }

            return hasMobConfig;
        }

        private MobConfig GetMobConfig(int index)
        {
            var mobConfig = _mobConfig;

            if (_mobSelectionMode == MobSpawnSelectionMode.List && _mobConfigs.Count != 0)
            {
                mobConfig = _mobConfigs[index % _mobConfigs.Count];
            }

            return mobConfig;
        }

        private bool CanSpawn(MobConfig mobConfig)
        {
            return mobConfig != null && mobConfig.HasPrefabKey;
        }
    }
}


