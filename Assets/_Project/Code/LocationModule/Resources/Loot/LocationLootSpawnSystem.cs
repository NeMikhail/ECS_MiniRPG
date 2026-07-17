using System.Collections.Generic;
using ECSMiniRPG.ContentManagement;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InventoryModule.Items;
using ECSMiniRPG.LocationModule.Resources.Components;
using ECSMiniRPG.LocationModule.Resources.Views;
using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.LocationModule.Resources.Loot
{
    public struct LocationLootSpawnSystem : ISystem
    {
        private List<LocationLootSpawnHandle> _spawnHandles;

        public void Update()
        {
            EnsureInitialized();
            ProcessCommands();
            CompleteSpawns();
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

        private void ReleaseSpawnHandle(ContentManagementSystem contentManagementSystem, LocationLootSpawnHandle spawnHandle)
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
        private void EnsureInitialized()
        {
            if (_spawnHandles == null)
            {
                _spawnHandles = new List<LocationLootSpawnHandle>();
            }
        }

        private void ProcessCommands()
        {
            ref var commandQueue = ref GameWorld.GetResource<LocationLootSpawnCommandQueue>();

            while (commandQueue.Count > 0)
            {
                StartSpawn(commandQueue.Dequeue());
            }
        }

        private void StartSpawn(LocationLootSpawnCommand command)
        {
            if (command._item != null && command._item.HasWorldLootPrefab && command._count > 0)
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
                var handle = contentManagementSystem.InstantiateAsync(command._item.WorldPresentation.LootPrefabKey, command._position, command._rotation);
                _spawnHandles.Add(new LocationLootSpawnHandle(command._item, command._count, handle));
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

        private void CompleteSpawn(LocationLootSpawnHandle spawnHandle)
        {
            ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();

            if (spawnHandle._handle.Status == AsyncOperationStatus.Succeeded)
            {
                var instance = spawnHandle._handle.Result;
                var lootView = instance.GetComponent<LootObjectView>();
                CreateEntity(instance, lootView, spawnHandle._item, spawnHandle._count);
            }

            if (spawnHandle._handle.Status != AsyncOperationStatus.Succeeded)
            {
                contentManagementSystem.Release(spawnHandle._handle);
            }
        }

        private void CreateEntity(GameObject instance, LootObjectView lootView, ItemPreset item, int count)
        {
            var entity = GameWorld.NewEntity<Default>();

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
                new LootObjectComponent
                {
                    _item = item,
                    _count = count,
                    _view = lootView
                }
            );
        }
    }
}