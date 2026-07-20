using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.GameplayModule.Events;
using ECSMiniRPG.GameplayModule.Runtime;
using ECSMiniRPG.LoggerModule;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.GameplayModule.Systems
{
    public struct HealthCommandSystem : ISystem
    {
        private EventReceiver<GameWorldType, HealthCommandEvent> _commandReceiver;

        public void Init()
        {
            _commandReceiver = GameWorld.RegisterEventReceiver<HealthCommandEvent>();
        }

        public void Update()
        {
            foreach (var commandEvent in _commandReceiver)
            {
                Process(commandEvent.Value);
            }
        }

        public void Destroy()
        {
            GameWorld.DeleteEventReceiver(ref _commandReceiver);
        }

        private void Process(HealthCommandEvent command)
        {
            var target = command._target;

            if (!target.Has<Health>())
            {
                LoggerSystem.Error(EcsModuleType.Gameplay, $"Health command target has no Health component. target={target} type={command._type} value={command._value}");
                return;
            }

            ref var health = ref target.Ref<Health>();

            var previousHealth = health._currentValue.Value;
            var previousMaxHealth = health._maxValue.Value;

            if (command._type == HealthCommandType.Damage)
            {
                ProcessDamage(target, ref health, command._value);
            }

            if (command._type == HealthCommandType.DirectDamage)
            {
                ProcessDirectDamage(target, ref health, command._value);
            }

            if (command._type == HealthCommandType.Heal)
            {
                ProcessHeal(target, ref health, command._value);
            }

            if (command._type == HealthCommandType.SetCurrent)
            {
                HealthOperations.SetCurrent(ref health, command._value);
            }

            if (command._type == HealthCommandType.SetMax)
            {
                HealthOperations.SetMax(ref health, command._value);
            }

            if (command._type == HealthCommandType.ChangeMax)
            {
                HealthOperations.ChangeMax(ref health, command._value);
            }

            if (command._type == HealthCommandType.Restore)
            {
                HealthOperations.Restore(ref health);
            }

            if (command._type == HealthCommandType.Kill)
            {
                HealthOperations.Kill(ref health);
            }

            LoggerSystem.Debug(EcsModuleType.Gameplay, $"Health command processed. target={target} type={command._type} value={command._value} hp={previousHealth}->{health._currentValue.Value}/{health._maxValue.Value}");
            SendChangeEvents(target, previousHealth, previousMaxHealth, health);
        }

        private static void ProcessDamage(GameWorld.Entity target, ref Health health, float value)
        {
            var finalDamage = DamageOperations.ClampDamage(value);

            if (target.Has<Armor>())
            {
                ref readonly var armor = ref target.Read<Armor>();
                finalDamage = DamageOperations.CalculateMitigatedDamage(value, armor._value.Value);
            }

            var appliedDamage = HealthOperations.ApplyDamage(ref health, finalDamage);
            GameWorld.SendEvent(new DamageTakenEvent
            {
                _target = target,
                _requestedDamage = DamageOperations.ClampDamage(value),
                _appliedDamage = appliedDamage
            });
        }

        private static void ProcessDirectDamage(GameWorld.Entity target, ref Health health, float value)
        {
            var appliedDamage = HealthOperations.ApplyDamage(ref health, value);
            GameWorld.SendEvent(new DamageTakenEvent
            {
                _target = target,
                _requestedDamage = DamageOperations.ClampDamage(value),
                _appliedDamage = appliedDamage
            });
        }

        private static void ProcessHeal(GameWorld.Entity target, ref Health health, float value)
        {
            var appliedHeal = HealthOperations.Heal(ref health, value);
            GameWorld.SendEvent(new HealedEvent
            {
                _target = target,
                _requestedHeal = Mathf.Max(0f, value),
                _appliedHeal = appliedHeal
            });
        }

        private static void SendChangeEvents(GameWorld.Entity target, float previousHealth, float previousMaxHealth, Health health)
        {
            var currentHealth = health._currentValue.Value;
            var currentMaxHealth = health._maxValue.Value;

            if (!Mathf.Approximately(previousMaxHealth, currentMaxHealth))
            {
                GameWorld.SendEvent(new MaxHealthChangedEvent
                {
                    _target = target,
                    _previousValue = previousMaxHealth,
                    _currentValue = currentMaxHealth
                });
            }

            if (!Mathf.Approximately(previousHealth, currentHealth))
            {
                GameWorld.SendEvent(new HealthChangedEvent
                {
                    _target = target,
                    _previousValue = previousHealth,
                    _currentValue = currentHealth,
                    _maxValue = currentMaxHealth
                });
            }

            if (previousHealth > 0f && Mathf.Approximately(currentHealth, 0f))
            {
                GameWorld.SendEvent<DeathEvent>();
            }
        }
    }
}