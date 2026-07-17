using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.GameplayModule.LongActions.Events;
using ECSMiniRPG.GameplayModule.Events;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.GameplayModule.LongActions.Systems
{
    public struct LongActionSystem : ISystem
    {
        private static readonly float _minDuration = 0f;
        private static readonly float _moveCancelSqrMagnitude = 0.0001f;

        private EventReceiver<GameWorldType, DamageTakenEvent> _damageReceiver;
        private EventReceiver<GameWorldType, LongActionInterruptEvent> _interruptReceiver;

        public void Init()
        {
            _damageReceiver = GameWorld.RegisterEventReceiver<DamageTakenEvent>();
            _interruptReceiver = GameWorld.RegisterEventReceiver<LongActionInterruptEvent>();
        }

        public void Update()
        {
            ProcessDamageEvents();
            ProcessInterruptEvents();
            ProcessCommands();
            ProcessActiveActions();
        }

        public void Destroy()
        {
            GameWorld.DeleteEventReceiver(ref _interruptReceiver);
            GameWorld.DeleteEventReceiver(ref _damageReceiver);
        }

        private void ProcessDamageEvents()
        {
            foreach (var damageEvent in _damageReceiver)
            {
                TryCancelByDamage(damageEvent.Value);
            }
        }

        private void ProcessInterruptEvents()
        {
            foreach (var interruptEvent in _interruptReceiver)
            {
                TryCancelByInterrupt(interruptEvent.Value);
            }
        }

        private void ProcessCommands()
        {
            ref var commandQueue = ref GameWorld.GetResource<LongActionCommandQueue>();

            while (commandQueue.Count > 0)
            {
                ProcessCommand(commandQueue.Dequeue());
            }
        }

        private void ProcessCommand(LongActionCommand command)
        {
            if (command._type == LongActionCommandType.Start)
            {
                StartAction(command);
            }

            if (command._type == LongActionCommandType.CancelOwner)
            {
                TryCancel(command._owner, command._cancelReason);
            }
        }

        private void StartAction(LongActionCommand command)
        {
            TryCancel(command._owner, LongActionCancelReason.Command);

            command._owner.Set(new LongActionComponent
            {
                _target = command._target,
                _duration = Mathf.Max(_minDuration, command._duration),
                _elapsed = 0f,
                _targetRange = Mathf.Max(0f, command._targetRange),
                _cancelPolicy = command._cancelPolicy,
                _behaviour = command._behaviour
            });

            GameWorld.SendEvent(new LongActionStartedEvent
            {
                _owner = command._owner,
                _target = command._target,
                _duration = command._duration
            });
        }

        private void ProcessActiveActions()
        {
            foreach (var entity in GameWorld.Query<All<LongActionComponent>>().Entities())
            {
                ProcessActiveAction(entity);
            }
        }

        private void ProcessActiveAction(GameWorld.Entity owner)
        {
            var hasCanceled = TryCancelByMove(owner);

            if (!hasCanceled)
            {
                hasCanceled = TryCancelByTargetRange(owner);
            }

            if (!hasCanceled)
            {
                TickAction(owner);
            }
        }

        private bool TryCancelByMove(GameWorld.Entity owner)
        {
            var hasCanceled = false;
            ref readonly var action = ref owner.Read<LongActionComponent>();

            if (ShouldCancelOnMove(owner, action))
            {
                hasCanceled = TryCancel(owner, LongActionCancelReason.Move);
            }

            return hasCanceled;
        }

        private bool ShouldCancelOnMove(GameWorld.Entity owner, LongActionComponent action)
        {
            var shouldCancel = false;

            if (action._cancelPolicy != null && action._cancelPolicy.ShouldCancelOnMove && owner.Has<PlayerInputData>())
            {
                ref readonly var inputData = ref owner.Read<PlayerInputData>();
                shouldCancel = inputData._moveDirection.sqrMagnitude > _moveCancelSqrMagnitude;
            }

            return shouldCancel;
        }

        private bool TryCancelByTargetRange(GameWorld.Entity owner)
        {
            var hasCanceled = false;
            ref readonly var action = ref owner.Read<LongActionComponent>();

            if (ShouldCancelOnTargetRange(owner, action))
            {
                hasCanceled = TryCancel(owner, LongActionCancelReason.TargetOutOfRange);
            }

            return hasCanceled;
        }

        private bool ShouldCancelOnTargetRange(GameWorld.Entity owner, LongActionComponent action)
        {
            var shouldCancel = false;

            if (action._cancelPolicy != null && action._cancelPolicy.ShouldCancelOnTargetOutOfRange && action._targetRange > 0f)
            {
                shouldCancel = IsTargetOutOfRange(owner, action);
            }

            return shouldCancel;
        }

        private bool IsTargetOutOfRange(GameWorld.Entity owner, LongActionComponent action)
        {
            var isOutOfRange = false;

            if (owner.Has<LongActionTransformRef>() && action._target.IsNotDestroyed && action._target.Has<LongActionTransformRef>())
            {
                ref readonly var ownerTransform = ref owner.Read<LongActionTransformRef>();
                ref readonly var targetTransform = ref action._target.Read<LongActionTransformRef>();
                var maxSqrDistance = action._targetRange * action._targetRange;
                var sqrDistance = (ownerTransform._value.position - targetTransform._value.position).sqrMagnitude;
                isOutOfRange = sqrDistance > maxSqrDistance;
            }

            return isOutOfRange;
        }

        private void TickAction(GameWorld.Entity owner)
        {
            ref var action = ref owner.Ref<LongActionComponent>();
            action._elapsed += Time.deltaTime;

            if (action._elapsed >= action._duration)
            {
                Complete(owner);
            }
        }

        private void Complete(GameWorld.Entity owner)
        {
            ref readonly var action = ref owner.Read<LongActionComponent>();
            var context = new LongActionContext(owner, action._target);
            var behaviour = action._behaviour;
            var target = action._target;

            owner.Delete<LongActionComponent>();

            if (behaviour != null)
            {
                behaviour.Complete(context);
            }

            GameWorld.SendEvent(new LongActionCompletedEvent
            {
                _owner = owner,
                _target = target
            });
        }

        private void TryCancelByDamage(DamageTakenEvent damageEvent)
        {
            if (damageEvent._target.IsNotDestroyed && damageEvent._target.Has<LongActionComponent>())
            {
                ref readonly var action = ref damageEvent._target.Read<LongActionComponent>();

                if (action._cancelPolicy != null && action._cancelPolicy.ShouldCancelOnDamage)
                {
                    TryCancel(damageEvent._target, LongActionCancelReason.Damage);
                }
            }
        }

        private void TryCancelByInterrupt(LongActionInterruptEvent interruptEvent)
        {
            if (interruptEvent._owner.IsNotDestroyed && interruptEvent._owner.Has<LongActionComponent>())
            {
                ref readonly var action = ref interruptEvent._owner.Read<LongActionComponent>();

                if (action._cancelPolicy != null && action._cancelPolicy.ShouldCancelOnAction)
                {
                    TryCancel(interruptEvent._owner, interruptEvent._reason);
                }
            }
        }

        private bool TryCancel(GameWorld.Entity owner, LongActionCancelReason reason)
        {
            var hasCanceled = false;

            if (owner.IsNotDestroyed && owner.Has<LongActionComponent>())
            {
                ref readonly var action = ref owner.Read<LongActionComponent>();
                var context = new LongActionContext(owner, action._target);
                var behaviour = action._behaviour;
                var target = action._target;

                owner.Delete<LongActionComponent>();

                if (behaviour != null)
                {
                    behaviour.Cancel(context, reason);
                }

                GameWorld.SendEvent(new LongActionCanceledEvent
                {
                    _owner = owner,
                    _target = target,
                    _reason = reason
                });

                hasCanceled = true;
            }

            return hasCanceled;
        }
    }
}
