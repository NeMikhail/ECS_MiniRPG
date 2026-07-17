using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.InventoryModule.Events;
using ECSMiniRPG.InventoryModule.Runtime;
using ECSMiniRPG.LocationModule.Resources.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Pickup
{
    public struct InventoryPickupSystem : ISystem
    {
        public void Update()
        {
            ref var commandQueue = ref GameWorld.GetResource<InventoryPickupCommandQueue>();

            while (commandQueue.Count > 0)
            {
                Process(commandQueue.Dequeue());
            }
        }

        private void Process(InventoryPickupCommand command)
        {
            if (CanProcess(command))
            {
                ProcessValidCommand(command);
            }
        }

        private bool CanProcess(InventoryPickupCommand command)
        {
            var canProcess = command._actor.IsNotDestroyed && command._target.IsNotDestroyed;

            if (canProcess)
            {
                canProcess = command._actor.Has<InventoryComponent>() && command._target.Has<LootObjectComponent>();
            }

            return canProcess;
        }

        private void ProcessValidCommand(InventoryPickupCommand command)
        {
            ref var inventory = ref command._actor.Ref<InventoryComponent>();
            ref var loot = ref command._target.Ref<LootObjectComponent>();
            var requestedCount = loot._count;
            var addedCount = InventoryOperations.AddItemPartial(inventory._state, loot._item, requestedCount);

            if (addedCount > 0)
            {
                inventory._isDirty = true;
                loot._count -= addedCount;
            }

            SendPickupEvent(command, loot, requestedCount, addedCount);

            if (loot._count <= 0)
            {
                command._target.Destroy();
            }
        }

        private void SendPickupEvent(InventoryPickupCommand command, LootObjectComponent loot, int requestedCount, int addedCount)
        {
            GameWorld.SendEvent(new InventoryPickupEvent
            {
                _actor = command._actor,
                _target = command._target,
                _item = loot._item,
                _requestedCount = requestedCount,
                _addedCount = addedCount,
                _remainingCount = loot._count,
                _resultType = ResolveResultType(requestedCount, addedCount),
                _sourceType = command._sourceType
            });
        }

        private InventoryPickupResultType ResolveResultType(int requestedCount, int addedCount)
        {
            var resultType = InventoryPickupResultType.Failed;

            if (addedCount > 0 && addedCount < requestedCount)
            {
                resultType = InventoryPickupResultType.Partial;
            }

            if (addedCount > 0 && addedCount >= requestedCount)
            {
                resultType = InventoryPickupResultType.Success;
            }

            return resultType;
        }
    }
}
