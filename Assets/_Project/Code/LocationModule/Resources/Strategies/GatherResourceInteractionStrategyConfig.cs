using ECSMiniRPG.GameplayModule.LongActions;
using ECSMiniRPG.InventoryModule.Tooling;
using ECSMiniRPG.InteractionModule;
using ECSMiniRPG.InteractionModule.Configs;
using ECSMiniRPG.LocationModule.Resources.Components;
using ECSMiniRPG.LocationModule.Resources.Events;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Strategies
{
    [CreateAssetMenu(fileName = "GatherResourceInteractionStrategyConfig", menuName = "ECSMiniRPG/Location/Gather Resource Interaction")]
    public sealed class GatherResourceInteractionStrategyConfig : InteractionStrategyConfig
    {
        [SerializeField] private GatherResourceLongActionBehaviourConfig _completeBehaviour;

        public override bool CanInteract(InteractionContext context)
        {
            var canInteract = false;

            if (context._target.IsNotDestroyed && context._target.Has<ResourceNodeComponent>())
            {
                ref readonly var resourceNode = ref context._target.Read<ResourceNodeComponent>();
                var hasActiveResource = resourceNode._state == LocationResourceNodeState.Active;
                var hasRequiredTool = HasRequiredTool(context, resourceNode);
                canInteract = hasActiveResource && hasRequiredTool;

                if (!hasActiveResource)
                {
                    SendFailedEvent(context, resourceNode, ResourceGatherFailReason.AlreadyGathered);
                }

                if (hasActiveResource && !hasRequiredTool)
                {
                    SendFailedEvent(context, resourceNode, ResourceGatherFailReason.MissingRequiredTool);
                }
            }

            return canInteract;
        }

        public override void Interact(InteractionContext context)
        {
            if (context._target.IsNotDestroyed && context._target.Has<ResourceNodeComponent>() && _completeBehaviour != null)
            {
                ref readonly var resourceNode = ref context._target.Read<ResourceNodeComponent>();
                var gatherDuration = resourceNode._config.GatherDuration;
                var gatherRange = resourceNode._config.GatherRange;
                GameWorld.SendEvent(new ResourceGatherStartedEvent
                {
                    _actor = context._actor,
                    _target = context._target,
                    _resource = resourceNode._config,
                    _duration = gatherDuration,
                    _range = gatherRange
                });
                ref var commandQueue = ref GameWorld.GetResource<LongActionCommandQueue>();
                commandQueue.EnqueueStart(
                    context._actor,
                    context._target,
                    gatherDuration,
                    gatherRange,
                    resourceNode._config.CancelPolicy,
                    _completeBehaviour
                );
            }
        }

        private bool HasRequiredTool(InteractionContext context, ResourceNodeComponent resourceNode)
        {
            var hasTool = resourceNode._config.RequiredTool == null;

            if (!hasTool && context._actor.Has<InventoryToolCache>())
            {
                ref readonly var toolCache = ref context._actor.Read<InventoryToolCache>();
                hasTool = toolCache.HasTool(resourceNode._config.RequiredTool);
            }

            return hasTool;
        }

        private void SendFailedEvent(InteractionContext context, ResourceNodeComponent resourceNode, ResourceGatherFailReason reason)
        {
            GameWorld.SendEvent(new ResourceGatherFailedEvent
            {
                _actor = context._actor,
                _target = context._target,
                _resource = resourceNode._config,
                _requiredTool = resourceNode._config.RequiredTool,
                _reason = reason
            });
        }
    }
}
