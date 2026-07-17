using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InputSystem;
using ECSMiniRPG.LocationModule.Resources.Components;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Pickup
{
    public struct InventoryPickupInputSystem : ISystem
    {
        public void Update()
        {
            ref var inputProvider = ref GameWorld.GetResource<NewInputProvider>();
            ref var runtimeData = ref GameWorld.GetResource<InventoryPickupRuntimeData>();

            if (inputProvider.IsPickUpPressed())
            {
                ProcessHold(runtimeData);
            }

            if (!inputProvider.IsPickUpPressed())
            {
                runtimeData.Reset();
            }
        }

        private void ProcessHold(InventoryPickupRuntimeData runtimeData)
        {
            ref var gameConfigs = ref GameWorld.GetResource<GameConfigs>();
            runtimeData.AddHoldTime(Time.deltaTime);
            runtimeData.AddRepeatTime(Time.deltaTime);

            if (CanPickUp(runtimeData, gameConfigs.InventoryConfig.PickupHoldDuration, gameConfigs.InventoryConfig.PickupRepeatInterval))
            {
                TryEnqueueNearestPickup(gameConfigs.InventoryConfig.PickupRadius);
                runtimeData.ResetRepeatTime();
            }
        }

        private bool CanPickUp(InventoryPickupRuntimeData runtimeData, float holdDuration, float repeatInterval)
        {
            var canPickUp = runtimeData.HoldTime >= holdDuration;

            if (canPickUp)
            {
                canPickUp = runtimeData.RepeatTime >= repeatInterval;
            }

            return canPickUp;
        }

        private void TryEnqueueNearestPickup(float pickupRadius)
        {
            ref var commandQueue = ref GameWorld.GetResource<InventoryPickupCommandQueue>();

            foreach (var player in GameWorld.Query<All<PlayerTag, LongActionTransformRef>>().Entities())
            {
                if (TryFindNearestLoot(player, pickupRadius, out var loot))
                {
                    commandQueue.Enqueue(player, loot, InventoryPickupSourceType.Button);
                }
            }
        }

        private bool TryFindNearestLoot(GameWorld.Entity actor, float pickupRadius, out GameWorld.Entity loot)
        {
            var hasLoot = false;
            var bestSqrDistance = float.MaxValue;
            var sqrRadius = pickupRadius * pickupRadius;
            loot = default;

            ref readonly var actorTransform = ref actor.Read<LongActionTransformRef>();

            foreach (var entity in GameWorld.Query<All<LootObjectComponent, LongActionTransformRef>>().Entities())
            {
                ref readonly var lootTransform = ref entity.Read<LongActionTransformRef>();
                var sqrDistance = (actorTransform._value.position - lootTransform._value.position).sqrMagnitude;

                if (sqrDistance <= sqrRadius && sqrDistance < bestSqrDistance)
                {
                    bestSqrDistance = sqrDistance;
                    loot = entity;
                    hasLoot = true;
                }
            }

            return hasLoot;
        }
    }
}
