using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.GameplayModule.Events;
using ECSMiniRPG.GameplayModule.Runtime;
using ECSMiniRPG.LoggerModule;
using ECSMiniRPG.MobModule.Components;
using ECSMiniRPG.MobModule.Views;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    public readonly struct MobAttackContext
    {
        public readonly GameWorld.Entity _mobEntity;
        public readonly GameWorld.Entity _targetEntity;
        public readonly MobView _view;
        public readonly Transform _targetTransform;
        public readonly bool _hasTarget;
        public readonly float _time;

        public MobAttackContext(GameWorld.Entity mobEntity, MobViewRef viewRef, MobTarget target, Transform targetTransform, float time)
        {
            _mobEntity = mobEntity;
            _targetEntity = target._value;
            _view = viewRef._value;
            _targetTransform = targetTransform;
            _hasTarget = target._hasValue && targetTransform != null && target._value.IsNotDestroyed && target._value.Has<Health>();
            _time = time;
        }

        public bool IsTargetInRange(float range)
        {
            var isInRange = false;
            var distance = 0f;

            if (_hasTarget && _view != null)
            {
                distance = Vector3.Distance(_targetTransform.position, _view.transform.position);
                isInRange = distance <= range;
            }

            LoggerSystem.Info(EcsModuleType.Mob, $"Attack range check. mob={_mobEntity} target={_targetEntity} hasTarget={_hasTarget} distance={distance} range={range} inRange={isInRange}", _view);
            return isInRange;
        }

        public void DamageTarget(float damage)
        {
            if (_hasTarget && _targetEntity.IsNotDestroyed && _targetEntity.Has<Health>())
            {
                ref var health = ref _targetEntity.Ref<Health>();
                var previousHealth = health._currentValue.Value;
                var finalDamage = DamageOperations.ClampDamage(damage);

                if (_targetEntity.Has<Armor>())
                {
                    ref readonly var armor = ref _targetEntity.Read<Armor>();
                    finalDamage = DamageOperations.CalculateMitigatedDamage(damage, armor._value.Value);
                }

                var appliedDamage = HealthOperations.ApplyDamage(ref health, finalDamage);
                LoggerSystem.Debug(EcsModuleType.Gameplay, $"Damage applied. target={_targetEntity} requested={damage} final={finalDamage} applied={appliedDamage} hp={previousHealth}->{health._currentValue.Value}/{health._maxValue.Value}", _view);
                SendDamageEvents(previousHealth, health, damage, appliedDamage);
            }

            if (!_hasTarget || !_targetEntity.IsNotDestroyed || !_targetEntity.Has<Health>())
            {
                LoggerSystem.Warning(EcsModuleType.Mob, $"Damage skipped. target={_targetEntity} hasTarget={_hasTarget} isNotDestroyed={_targetEntity.IsNotDestroyed} hasHealth={_targetEntity.IsNotDestroyed && _targetEntity.Has<Health>()}", _view);
            }
        }

        private void SendDamageEvents(float previousHealth, Health health, float requestedDamage, float appliedDamage)
        {
            GameWorld.SendEvent(new DamageTakenEvent
            {
                _target = _targetEntity,
                _requestedDamage = DamageOperations.ClampDamage(requestedDamage),
                _appliedDamage = appliedDamage
            });

            if (!Mathf.Approximately(previousHealth, health._currentValue.Value))
            {
                GameWorld.SendEvent(new HealthChangedEvent
                {
                    _target = _targetEntity,
                    _previousValue = previousHealth,
                    _currentValue = health._currentValue.Value,
                    _maxValue = health._maxValue.Value
                });
            }

            if (previousHealth > 0f && Mathf.Approximately(health._currentValue.Value, 0f))
            {
                GameWorld.SendEvent<DeathEvent>();
            }
        }
    }
}