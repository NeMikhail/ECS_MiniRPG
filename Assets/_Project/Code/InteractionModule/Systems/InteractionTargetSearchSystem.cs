using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InteractionModule.Components;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.InteractionModule.Systems
{
    public struct InteractionTargetSearchSystem : ISystem
    {
        public void Update()
        {
            foreach (var actor in GameWorld.Query<All<PlayerTag, LongActionTransformRef>>().Entities())
            {
                UpdateTarget(actor);
            }
        }

        private void UpdateTarget(GameWorld.Entity actor)
        {
            var hasTarget = TryFindTarget(actor, out var target);

            if (hasTarget)
            {
                actor.Set(new InteractionTarget
                {
                    _value = target
                });
            }

            if (!hasTarget && actor.Has<InteractionTarget>())
            {
                actor.Delete<InteractionTarget>();
            }
        }

        private bool TryFindTarget(GameWorld.Entity actor, out GameWorld.Entity target)
        {
            var hasTarget = false;
            var bestSqrDistance = float.MaxValue;
            target = default;

            ref readonly var actorTransform = ref actor.Read<LongActionTransformRef>();

            foreach (var interactable in GameWorld.Query<All<InteractableTag, InteractionViewRef, LongActionTransformRef>>().Entities())
            {
                ref readonly var interactableView = ref interactable.Read<InteractionViewRef>();
                ref readonly var interactableTransform = ref interactable.Read<LongActionTransformRef>();
                var sqrDistance = (actorTransform._value.position - interactableTransform._value.position).sqrMagnitude;
                var radius = interactableView._value.InteractionRadius;
                var sqrRadius = radius * radius;

                if (sqrDistance <= sqrRadius && sqrDistance < bestSqrDistance)
                {
                    target = interactable;
                    bestSqrDistance = sqrDistance;
                    hasTarget = true;
                }
            }

            return hasTarget;
        }
    }
}
