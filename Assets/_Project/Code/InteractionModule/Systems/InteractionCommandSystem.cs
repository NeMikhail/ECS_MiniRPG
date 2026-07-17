using ECSMiniRPG.InteractionModule.Components;
using ECSMiniRPG.InteractionModule.Events;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InteractionModule.Systems
{
    public struct InteractionCommandSystem : ISystem
    {
        public void Update()
        {
            ref var commandQueue = ref GameWorld.GetResource<InteractionCommandQueue>();

            while (commandQueue.Count > 0)
            {
                Process(commandQueue.Dequeue());
            }
        }

        private void Process(GameWorld.Entity actor)
        {
            if (actor.IsNotDestroyed && actor.Has<InteractionTarget>())
            {
                ref readonly var interactionTarget = ref actor.Read<InteractionTarget>();
                TryInteract(actor, interactionTarget._value);
            }

            if (actor.IsNotDestroyed && !actor.Has<InteractionTarget>())
            {
                SendFailedEvent(actor, default, InteractionFailReason.NoTarget);
            }
        }

        private void TryInteract(GameWorld.Entity actor, GameWorld.Entity target)
        {
            if (target.IsDestroyed || !target.Has<InteractionViewRef>())
            {
                SendFailedEvent(actor, target, InteractionFailReason.TargetUnavailable);
                return;
            }

            ref readonly var viewRef = ref target.Read<InteractionViewRef>();
            var strategy = viewRef._value.Strategy;
            var context = new InteractionContext(actor, target);

            if (strategy == null)
            {
                SendFailedEvent(actor, target, InteractionFailReason.MissingStrategy);
                return;
            }

            var canInteract = strategy.CanInteract(context);

            if (canInteract)
            {
                strategy.Interact(context);
            }

            if (!canInteract)
            {
                SendFailedEvent(actor, target, InteractionFailReason.RequirementsNotMet);
            }
        }

        private void SendFailedEvent(GameWorld.Entity actor, GameWorld.Entity target, InteractionFailReason reason)
        {
            GameWorld.SendEvent(new InteractionFailedEvent
            {
                _actor = actor,
                _target = target,
                _reason = reason
            });
        }
    }
}
