using System.Collections.Generic;
using ECSMiniRPG.ContentManagement;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.GUIModule.Components;
using ECSMiniRPG.MobModule.Components;
using ECSMiniRPG.MobModule.Configs;
using ECSMiniRPG.MobModule.Spawn;
using ECSMiniRPG.MobModule.Views;
using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.MobModule.Systems
{
    public struct MobSpawnSystem : ISystem
    {
        private static readonly float _navMeshSampleDistance = 10f;

        private List<MobSpawnHandle> _spawnHandles;

        public void Update()
        {
            EnsureInitialized();
            CompleteSpawns();
            ProcessCommands();
        }

        public void Destroy()
        {
            if (_spawnHandles != null && GameWorld.IsWorldInitialized)
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();

                for (var i = 0; i < _spawnHandles.Count; i++)
                {
                    ReleaseSpawnHandle(contentManagementSystem, _spawnHandles[i]);
                }
            }
        }

        private void EnsureInitialized()
        {
            if (_spawnHandles == null)
            {
                _spawnHandles = new List<MobSpawnHandle>();
            }
        }

        private void ProcessCommands()
        {
            ref var commandQueue = ref GameWorld.GetResource<MobSpawnCommandQueue>();

            while (commandQueue.Count > 0)
            {
                StartSpawn(commandQueue.Dequeue());
            }
        }

        private void StartSpawn(MobSpawnCommand command)
        {
            if (command._config != null && command._config.HasPrefabKey)
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
                var spawnPosition = GetNavMeshSpawnPosition(command._position);
                var handle = contentManagementSystem.InstantiateAsync(command._config.PrefabKey, spawnPosition, command._rotation);
                _spawnHandles.Add(new MobSpawnHandle(command._config, command._spawner, handle));
            }
        }

        private Vector3 GetNavMeshSpawnPosition(Vector3 position)
        {
            var spawnPosition = position;

            if (NavMesh.SamplePosition(position, out var hit, _navMeshSampleDistance, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
            }

            return spawnPosition;
        }

        private void CompleteSpawns()
        {
            for (var i = _spawnHandles.Count - 1; i >= 0; i--)
            {
                if (_spawnHandles[i]._handle.IsDone)
                {
                    CompleteSpawn(_spawnHandles[i]);
                    _spawnHandles.RemoveAt(i);
                }
            }
        }

        private void CompleteSpawn(MobSpawnHandle spawnHandle)
        {
            ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
            var hasSpawned = false;

            if (spawnHandle._handle.Status == AsyncOperationStatus.Succeeded)
            {
                var entity = CreateMobEntity(spawnHandle._config, spawnHandle._handle.Result);
                RegisterSpawnerMob(spawnHandle._spawner, entity);
                hasSpawned = true;
            }

            if (spawnHandle._handle.Status == AsyncOperationStatus.Succeeded && !hasSpawned)
            {
                contentManagementSystem.ReleaseInstance(spawnHandle._handle.Result);
            }

            if (spawnHandle._handle.Status != AsyncOperationStatus.Succeeded)
            {
                contentManagementSystem.Release(spawnHandle._handle);
            }
        }

        private GameWorld.Entity CreateMobEntity(MobConfig config, GameObject instance)
        {
            var mobView = instance.GetComponentInChildren<MobView>(true);

            if (mobView == null)
            {
                mobView = instance.AddComponent<BasicEnemy>();
            }

            mobView.SetConfig(config);

            var entity = GameWorld.NewEntity<Default>();
            var health = Health.Create(config.MaxHealth);
            var armor = Armor.Create(config.Armor);

            entity.Set(
                health,
                armor,
                new MobConfigRef
                {
                    _value = config
                },
                new MobViewRef
                {
                    _value = mobView
                },
                new MobContentRef
                {
                    _instance = instance,
                    _isAddressableInstance = true
                },
                new LongActionTransformRef
                {
                    _value = instance.transform
                },
                new MobTarget(),
                new MobMovementState(),
                new MobAttackState()
            );
            entity.Set<MobTag>();

            if (mobView.HealthBarView != null)
            {
                if (mobView.HealthBarCamera != null)
                {
                    mobView.HealthBarView.SetCamera(mobView.HealthBarCamera);
                }

                entity.Set(new HealthBarViewRef
                {
                    _value = mobView.HealthBarView
                });
            }

            return entity;
        }

        private void RegisterSpawnerMob(MobSpawnerView spawner, GameWorld.Entity entity)
        {
            if (spawner != null)
            {
                spawner.AddSpawnedMob(entity);
            }
        }

        private void ReleaseSpawnHandle(ContentManagementSystem contentManagementSystem, MobSpawnHandle spawnHandle)
        {
            if (spawnHandle._handle.IsDone && spawnHandle._handle.Status == AsyncOperationStatus.Succeeded && spawnHandle._handle.Result != null)
            {
                contentManagementSystem.ReleaseInstance(spawnHandle._handle.Result);
            }

            if (!spawnHandle._handle.IsDone || spawnHandle._handle.Status != AsyncOperationStatus.Succeeded)
            {
                contentManagementSystem.Release(spawnHandle._handle);
            }
        }
    }
}