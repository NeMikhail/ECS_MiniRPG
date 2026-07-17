using System.Collections.Generic;
using ECSMiniRPG.ContentManagement;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InteractionModule.Components;
using ECSMiniRPG.InteractionModule.Views;
using ECSMiniRPG.LocationModule.Resources.Components;
using ECSMiniRPG.LocationModule.Resources.Configs;
using ECSMiniRPG.LocationModule.Resources.Views;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.LocationModule.Resources.Systems
{
    public struct LocationResourceSpawnSystem : ISystem
    {
        private static readonly float _emptyTimerValue = 0f;

        private List<LocationResourceSpawnHandle> _spawnHandles;

        public void Update()
        {
            EnsureInitialized();
            CompleteSpawns();
            ref var locationRuntimeData = ref GameWorld.GetResource<LocationRuntimeData>();

            for (var i = 0; i < locationRuntimeData.ResourceSpawnPoints.Count; i++)
            {
                UpdateSpawnPoint(locationRuntimeData.ResourceSpawnPoints[i]);
            }
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
                _spawnHandles = new List<LocationResourceSpawnHandle>();
            }
        }

        private void ReleaseSpawnHandle(ContentManagementSystem contentManagementSystem, LocationResourceSpawnHandle spawnHandle)
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

        private void CompleteSpawn(LocationResourceSpawnHandle spawnHandle)
        {
            ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
            var hasSpawned = false;

            if (spawnHandle._handle.Status == AsyncOperationStatus.Succeeded && !spawnHandle._spawnPoint.HasActiveEntity)
            {
                SpawnResource(spawnHandle._spawnPoint, spawnHandle._resource, spawnHandle._handle.Result);
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

        private void UpdateSpawnPoint(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            ClearDestroyedActiveEntity(spawnPoint);
            TickTimers(spawnPoint);

            if (ShouldRefresh(spawnPoint) && IsOutsidePlayerVisibility(spawnPoint))
            {
                RefreshResourceType(spawnPoint);
            }

            if (!spawnPoint.HasActiveEntity && spawnPoint.SpawnTimer <= _emptyTimerValue && !HasPendingSpawn(spawnPoint))
            {
                TrySpawnCurrentResource(spawnPoint);
            }

            if (spawnPoint.HasActiveEntity && spawnPoint.RestoreTimer <= _emptyTimerValue)
            {
                TryRestoreActiveResource(spawnPoint);
            }
        }

        private void ClearDestroyedActiveEntity(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            if (spawnPoint.HasActiveEntity && spawnPoint.ActiveEntity.IsDestroyed)
            {
                spawnPoint.ClearActiveEntity();
            }
        }

        private void TickTimers(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            spawnPoint.SetSpawnTimer(Mathf.Max(_emptyTimerValue, spawnPoint.SpawnTimer - Time.deltaTime));
            spawnPoint.SetRestoreTimer(Mathf.Max(_emptyTimerValue, spawnPoint.RestoreTimer - Time.deltaTime));
            spawnPoint.SetRefreshTimer(Mathf.Max(_emptyTimerValue, spawnPoint.RefreshTimer - Time.deltaTime));
        }

        private bool ShouldRefresh(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            return spawnPoint.Config.ResourceRefreshInterval > _emptyTimerValue && spawnPoint.RefreshTimer <= _emptyTimerValue;
        }

        private bool IsOutsidePlayerVisibility(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            var isOutside = true;
            var visibilityRadius = spawnPoint.Config.PlayerVisibilityRadius;
            var visibilitySqrRadius = visibilityRadius * visibilityRadius;
            var spawnPointPosition = spawnPoint.View.transform.position;

            foreach (var player in GameWorld.Query<All<PlayerTag, LongActionTransformRef>>().Entities())
            {
                ref readonly var playerTransform = ref player.Read<LongActionTransformRef>();
                var sqrDistance = (playerTransform._value.position - spawnPointPosition).sqrMagnitude;

                if (sqrDistance <= visibilitySqrRadius)
                {
                    isOutside = false;
                }
            }

            return isOutside;
        }

        private void RefreshResourceType(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            DestroyActiveResource(spawnPoint);
            CancelPendingSpawn(spawnPoint);
            spawnPoint.SetCurrentResource(SelectResource(spawnPoint.Config));
            spawnPoint.ResetRefreshTimer();
            spawnPoint.SetSpawnTimer(_emptyTimerValue);
            spawnPoint.SetRestoreTimer(_emptyTimerValue);
            TrySpawnCurrentResource(spawnPoint);
        }

        private ResourceNodeConfig SelectResource(LocationResourceSpawnPointConfig config)
        {
            ResourceNodeConfig resource = null;
            var totalWeight = CalculateTotalWeight(config);

            if (totalWeight > 0)
            {
                resource = SelectWeightedResource(config, Random.Range(0, totalWeight));
            }

            return resource;
        }

        private int CalculateTotalWeight(LocationResourceSpawnPointConfig config)
        {
            var totalWeight = 0;

            for (var i = 0; i < config.Resources.Count; i++)
            {
                totalWeight += config.Resources[i].Weight;
            }

            return totalWeight;
        }

        private ResourceNodeConfig SelectWeightedResource(LocationResourceSpawnPointConfig config, int roll)
        {
            ResourceNodeConfig resource = null;
            var currentWeight = 0;
            var hasSelected = false;

            for (var i = 0; i < config.Resources.Count; i++)
            {
                if (!hasSelected)
                {
                    currentWeight += config.Resources[i].Weight;

                    if (roll < currentWeight)
                    {
                        resource = config.Resources[i].Resource;
                        hasSelected = true;
                    }
                }
            }

            return resource;
        }

        private void TrySpawnCurrentResource(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            if (!spawnPoint.HasCurrentResourceSelection)
            {
                spawnPoint.SetCurrentResource(SelectResource(spawnPoint.Config));
            }

            if (spawnPoint.CurrentResource != null && spawnPoint.CurrentResource.HasPrefab && !spawnPoint.HasActiveEntity && !HasPendingSpawn(spawnPoint))
            {
                StartResourceSpawn(spawnPoint, spawnPoint.CurrentResource);
            }
        }

        private void StartResourceSpawn(LocationResourceSpawnPointRuntimeData spawnPoint, ResourceNodeConfig resource)
        {
            ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
            var handle = contentManagementSystem.InstantiateAsync(resource.PrefabKey, spawnPoint.View.transform.position, spawnPoint.View.transform.rotation);
            _spawnHandles.Add(new LocationResourceSpawnHandle(spawnPoint, resource, handle));
        }

        private void SpawnResource(LocationResourceSpawnPointRuntimeData spawnPoint, ResourceNodeConfig resource, GameObject instance)
        {
            var resourceView = instance.GetComponentInChildren<ResourceNodeView>(true);
            var interactableView = instance.GetComponentInChildren<InteractableView>(true);
            var entity = GameWorld.NewEntity<Default>();

            if (resourceView != null)
            {
                resourceView.ApplyState(LocationResourceNodeState.Active);
            }

            entity.Set(
                new LocationObjectContentRef
                {
                    _instance = instance,
                    _isAddressableInstance = true
                },
                new LongActionTransformRef
                {
                    _value = instance.transform
                },
                new ResourceNodeComponent
                {
                    _config = resource,
                    _spawnPoint = spawnPoint,
                    _view = resourceView,
                    _state = LocationResourceNodeState.Active
                }
            );

            if (interactableView != null)
            {
                entity.Set(new InteractionViewRef
                {
                    _value = interactableView
                });
                entity.Set<InteractableTag>();
            }

            spawnPoint.SetActiveEntity(entity);
        }

        private void TryRestoreActiveResource(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            if (spawnPoint.ActiveEntity.Has<ResourceNodeComponent>())
            {
                ref var resourceNode = ref spawnPoint.ActiveEntity.Ref<ResourceNodeComponent>();

                if (resourceNode._state == LocationResourceNodeState.Depleted)
                {
                    resourceNode._state = LocationResourceNodeState.Active;

                    if (spawnPoint.ActiveEntity.Has<InteractionViewRef>())
                    {
                        spawnPoint.ActiveEntity.Set<InteractableTag>();
                    }

                    if (resourceNode._view != null)
                    {
                        resourceNode._view.ApplyState(LocationResourceNodeState.Active);
                    }
                }
            }
        }

        private bool HasPendingSpawn(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            var hasPending = false;

            for (var i = 0; i < _spawnHandles.Count; i++)
            {
                if (_spawnHandles[i]._spawnPoint == spawnPoint)
                {
                    hasPending = true;
                }
            }

            return hasPending;
        }

        private void CancelPendingSpawn(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();

            for (var i = _spawnHandles.Count - 1; i >= 0; i--)
            {
                if (_spawnHandles[i]._spawnPoint == spawnPoint)
                {
                    ReleaseSpawnHandle(contentManagementSystem, _spawnHandles[i]);
                    _spawnHandles.RemoveAt(i);
                }
            }
        }

        private void DestroyActiveResource(LocationResourceSpawnPointRuntimeData spawnPoint)
        {
            if (spawnPoint.HasActiveEntity && spawnPoint.ActiveEntity.IsNotDestroyed)
            {
                spawnPoint.ActiveEntity.Destroy();
            }

            spawnPoint.ClearActiveEntity();
        }
    }
}