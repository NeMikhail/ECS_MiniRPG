using ECSMiniRPG.GameplayModule.LongActions;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InteractionModule.Components;
using ECSMiniRPG.InventoryModule.Items;
using ECSMiniRPG.LocationModule.Resources.Components;
using ECSMiniRPG.LocationModule.Resources.Configs;
using ECSMiniRPG.LocationModule.Resources.Loot;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Strategies
{
    [CreateAssetMenu(fileName = "GatherResourceLongActionBehaviourConfig", menuName = "ECSMiniRPG/Location/Gather Resource Long Action")]
    public sealed class GatherResourceLongActionBehaviourConfig : LongActionBehaviourConfig
    {
        [SerializeField] private float _lootSpawnRadius = 0.5f;

        public override void Complete(LongActionContext context)
        {
            if (context._target.IsNotDestroyed && context._target.Has<ResourceNodeComponent>())
            {
                DepleteResource(context._target);
                SpawnLoot(context._target);
            }
        }

        private void DepleteResource(GameWorld.Entity resourceEntity)
        {
            ref var resourceNode = ref resourceEntity.Ref<ResourceNodeComponent>();
            resourceNode._state = LocationResourceNodeState.Depleted;
            resourceNode._spawnPoint.SetRestoreTimer(resourceNode._spawnPoint.Config.DepletedRestoreDelay);
            resourceEntity.Delete<InteractableTag>();

            if (resourceNode._view != null)
            {
                resourceNode._view.ApplyState(LocationResourceNodeState.Depleted);
            }
        }

        private void SpawnLoot(GameWorld.Entity resourceEntity)
        {
            ref readonly var resourceNode = ref resourceEntity.Read<ResourceNodeComponent>();

            for (var i = 0; i < resourceNode._config.Loot.Count; i++)
            {
                SpawnLoot(resourceEntity, resourceNode._config.Loot[i]);
            }
        }

        private void SpawnLoot(GameWorld.Entity resourceEntity, ResourceLootEntryConfig lootEntry)
        {
            if (lootEntry.Item != null && lootEntry.Item.HasWorldLootPrefab)
            {
                var count = Random.Range(lootEntry.MinCount, lootEntry.MaxCount + 1);

                if (count > 0)
                {
                    EnqueueLootSpawn(resourceEntity, lootEntry.Item, count);
                }
            }
        }

        private void EnqueueLootSpawn(GameWorld.Entity resourceEntity, ItemPreset item, int count)
        {
            ref readonly var transformRef = ref resourceEntity.Read<LongActionTransformRef>();
            ref var commandQueue = ref GameWorld.GetResource<LocationLootSpawnCommandQueue>();
            var position = transformRef._value.position + Random.insideUnitSphere * Mathf.Max(0f, _lootSpawnRadius);
            commandQueue.Enqueue(item, count, position, Quaternion.identity);
        }
    }
}