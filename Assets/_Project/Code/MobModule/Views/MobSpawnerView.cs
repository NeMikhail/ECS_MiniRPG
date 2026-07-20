using System.Collections.Generic;
using ECSMiniRPG.MobModule.Strategies;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Views
{
    public sealed class MobSpawnerView : MonoBehaviour
    {
        private static readonly List<MobSpawnerView> _spawners = new List<MobSpawnerView>();

        [SerializeField] private MobSpawnBehaviourConfig _spawnBehaviour;

        private readonly List<GameWorld.Entity> _spawnedMobs = new List<GameWorld.Entity>();
        private float _nextSpawnTime;
        private int _waveIndex;

        public static IReadOnlyList<MobSpawnerView> Spawners => _spawners;
        public MobSpawnBehaviourConfig SpawnBehaviour => _spawnBehaviour;
        public IReadOnlyList<GameWorld.Entity> SpawnedMobs => _spawnedMobs;
        public float NextSpawnTime { get => _nextSpawnTime; set => _nextSpawnTime = value; }
        public int WaveIndex { get => _waveIndex; set => _waveIndex = value; }

        private void OnEnable()
        {
            if (!_spawners.Contains(this))
            {
                _spawners.Add(this);
            }
        }

        private void OnDisable()
        {
            _spawners.Remove(this);
        }

        public void AddSpawnedMob(GameWorld.Entity mob)
        {
            if (!_spawnedMobs.Contains(mob))
            {
                _spawnedMobs.Add(mob);
            }
        }

        public void RemoveDestroyedMobs()
        {
            for (var i = _spawnedMobs.Count - 1; i >= 0; i--)
            {
                if (_spawnedMobs[i].IsDestroyed)
                {
                    _spawnedMobs.RemoveAt(i);
                }
            }
        }

        public Vector3 GetRandomPointInRadius(float radius)
        {
            var offset = Random.insideUnitCircle * radius;
            return transform.position + new Vector3(offset.x, 0f, offset.y);
        }
    }
}
