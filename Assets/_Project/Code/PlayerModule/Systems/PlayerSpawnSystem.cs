using System;
using ECSMiniRPG.ContentManagement;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.GUIModule.Components;
using ECSMiniRPG.LocationModule;
using ECSMiniRPG.PlayerModule.Components;
using ECSMiniRPG.PlayerModule.Configs;
using ECSMiniRPG.PlayerModule.Views;
using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.PlayerModule.Systems
{
    public struct PlayerSpawnSystem : ISystem
    {
        private AsyncOperationHandle<GameObject> _spawnHandle;
        private bool _isSpawnStarted;
        private bool _hasSpawned;
        private bool _hasSpawnFailed;

        public void Update()
        {
            if (!_isSpawnStarted && !_hasSpawnFailed)
            {
                StartSpawn();
            }

            if (_isSpawnStarted && !_hasSpawned && _spawnHandle.IsDone)
            {
                CompleteSpawn();
            }
        }

        private void StartSpawn()
        {
            if (TryResolveSpawnPose(out var spawnPose))
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
                _spawnHandle = contentManagementSystem.InstantiateAsync(
                    ContentKeys.PlayerPrefab,
                    spawnPose.Position,
                    spawnPose.Rotation
                );
                _isSpawnStarted = true;
            }

            if (!_isSpawnStarted)
            {
                _hasSpawnFailed = true;
            }
        }

        private bool TryResolveSpawnPose(out PlayerSpawnPose spawnPose)
        {
            ref var gameConfigs = ref GameWorld.GetResource<GameConfigs>();
            ref var spawnRuntimeData = ref GameWorld.GetResource<PlayerSpawnRuntimeData>();
            LocationRuntimeData locationRuntimeData = null;
            var hasSpawnPose = false;
            spawnPose = PlayerSpawnPose.Identity;

            if (spawnRuntimeData.CurrentStrategy == null)
            {
                Debug.LogError("Player spawn strategy is not assigned.");
            }

            if (spawnRuntimeData.CurrentStrategy != null)
            {
                hasSpawnPose = TryResolveSpawnPose(spawnRuntimeData.CurrentStrategy, gameConfigs, out locationRuntimeData, out spawnPose);
            }

            return hasSpawnPose;
        }

        private bool TryResolveSpawnPose(
            PlayerSpawnStrategyConfig spawnStrategy,
            GameConfigs gameConfigs,
            out LocationRuntimeData locationRuntimeData,
            out PlayerSpawnPose spawnPose
        )
        {
            var hasLocationRuntimeData = true;
            var hasSpawnPose = false;
            locationRuntimeData = null;
            spawnPose = PlayerSpawnPose.Identity;

            if (spawnStrategy.RequiresLocationData)
            {
                hasLocationRuntimeData = TryGetLocationRuntimeData(out locationRuntimeData);
            }

            if (hasLocationRuntimeData)
            {
                var resolveContext = new PlayerSpawnResolveContext(gameConfigs, locationRuntimeData);
                hasSpawnPose = spawnStrategy.TryResolve(resolveContext, out spawnPose);
            }

            return hasSpawnPose;
        }

        private bool TryGetLocationRuntimeData(out LocationRuntimeData locationRuntimeData)
        {
            var hasLocationRuntimeData = false;
            locationRuntimeData = null;

            try
            {
                locationRuntimeData = GameWorld.GetResource<LocationRuntimeData>();
                hasLocationRuntimeData = true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"Location runtime data is not registered. Enable Location module before Player module. {exception.Message}");
            }

            return hasLocationRuntimeData;
        }

        private void CompleteSpawn()
        {
            if (_spawnHandle.Status == AsyncOperationStatus.Succeeded)
            {
                CreatePlayerEntity(_spawnHandle.Result);
            }

            if (_spawnHandle.Status != AsyncOperationStatus.Succeeded)
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
                contentManagementSystem.Release(_spawnHandle);
            }

            _hasSpawned = true;
        }

        private void CreatePlayerEntity(GameObject instance)
        {
            ref var gameConfigs = ref GameWorld.GetResource<GameConfigs>();
            var playerView = instance.GetComponent<PlayerView>();
            var entity = GameWorld.NewEntity<Default>();
            var health = Health.Create(gameConfigs.PlayerConfig.MaxHealth);

            entity.Set(
                health,
                new PlayerInputData(),
                new PlayerMoveSpeed
                {
                    _value = gameConfigs.PlayerConfig.MoveSpeed
                },
                new PlayerViewRef
                {
                    _value = playerView
                },
                new PlayerContentRef
                {
                    _instance = instance
                },
                InventoryComponent.Create(),
                Armor.Create(0f),
                new InventoryEquipmentStats()
            );
            entity.Set<PlayerTag>();

            if (playerView.HealthBarView != null)
            {
                playerView.HealthBarView.SetCamera(playerView.Camera);
                playerView.HealthBarView.Bind(health);
                entity.Set(new HealthBarViewRef
                {
                    _value = playerView.HealthBarView
                });
            }
        }

        public void Destroy()
        {
            if (_isSpawnStarted && !_hasSpawned)
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
                contentManagementSystem.Release(_spawnHandle);
            }
        }
    }
}

