using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.InventoryModule.Events;
using ECSMiniRPG.InventoryModule.Items;
using ECSMiniRPG.InventoryModule.Runtime;
using ECSMiniRPG.LocationModule.Resources.Loot;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Drop
{
    public struct InventoryDropSystem : ISystem
    {
        public void Update()
        {
            ref var commandQueue = ref GameWorld.GetResource<InventoryDropCommandQueue>();

            while (commandQueue.Count > 0)
            {
                Process(commandQueue.Dequeue());
            }
        }

        private void Process(InventoryDropCommand command)
        {
            if (CanProcess(command))
            {
                ProcessValidCommand(command);
            }
        }

        private bool CanProcess(InventoryDropCommand command)
        {
            var canProcess = command._actor.IsNotDestroyed && command._actor.Has<InventoryComponent>();

            if (canProcess)
            {
                canProcess = command._count > 0;
            }

            return canProcess;
        }

        private void ProcessValidCommand(InventoryDropCommand command)
        {
            ref var gameConfigs = ref GameWorld.GetResource<GameConfigs>();
            ref var inventory = ref command._actor.Ref<InventoryComponent>();
            var item = InventoryOperations.GetItem(inventory._state, command._sourceData);
            var failReason = Validate(command, item);

            if (failReason == InventoryDropFailReason.None)
            {
                Drop(command, ref inventory, item, gameConfigs.InventoryConfig.DropRadius);
            }

            if (failReason != InventoryDropFailReason.None)
            {
                SendEvent(command, item != null ? item.Preset : null, 0, InventoryDropResultType.Failed, failReason);
            }
        }

        private InventoryDropFailReason Validate(InventoryDropCommand command, InventoryItemInstance item)
        {
            var failReason = InventoryDropFailReason.None;

            if (item == null)
            {
                failReason = InventoryDropFailReason.EmptySlot;
            }

            if (failReason == InventoryDropFailReason.None && !item.Preset.HasWorldLootPrefab)
            {
                failReason = InventoryDropFailReason.MissingLootConfig;
            }

            if (failReason == InventoryDropFailReason.None && !command._actor.Has<LongActionTransformRef>())
            {
                failReason = InventoryDropFailReason.MissingActorTransform;
            }

            return failReason;
        }

        private void Drop(InventoryDropCommand command, ref InventoryComponent inventory, InventoryItemInstance item, float dropRadius)
        {
            var requestedCount = Mathf.Min(command._count, item.Count);
            var removedCount = InventoryOperations.RemoveItem(inventory._state, command._sourceData, requestedCount);

            if (removedCount > 0)
            {
                inventory._isDirty = true;
                EnqueueLootSpawn(command._actor, item.Preset, removedCount, dropRadius);
                SendEvent(command, item.Preset, removedCount, InventoryDropResultType.Success, InventoryDropFailReason.None);
            }

            if (removedCount <= 0)
            {
                SendEvent(command, item.Preset, 0, InventoryDropResultType.Failed, InventoryDropFailReason.InvalidSource);
            }
        }

        private void EnqueueLootSpawn(GameWorld.Entity actor, ItemPreset item, int count, float dropRadius)
        {
            ref readonly var transformRef = ref actor.Read<LongActionTransformRef>();
            ref var commandQueue = ref GameWorld.GetResource<LocationLootSpawnCommandQueue>();
            var offset = Random.insideUnitSphere * Mathf.Max(0f, dropRadius);
            offset.y = 0f;
            commandQueue.Enqueue(item, count, transformRef._value.position + offset, Quaternion.identity);
        }

        private void SendEvent(InventoryDropCommand command, ItemPreset item, int droppedCount, InventoryDropResultType resultType, InventoryDropFailReason failReason)
        {
            GameWorld.SendEvent(new InventoryDropEvent
            {
                _actor = command._actor,
                _item = item,
                _sourceData = command._sourceData,
                _requestedCount = command._count,
                _droppedCount = droppedCount,
                _resultType = resultType,
                _sourceType = command._sourceType,
                _failReason = failReason
            });
        }
    }
}