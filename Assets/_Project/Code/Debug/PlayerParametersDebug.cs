using System;
using ECSMiniRPG.GameplayModule;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.InventoryModule.Runtime;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ECSMiniRPG.DebugTools
{
    public sealed class PlayerParametersDebug : MonoBehaviour
    {
        private static readonly string _playModeError = "Player parameters debug works only in Play Mode.";
        private static readonly float _minHealthValue = 0f;
        private static readonly float _minSpeedValue = 0f;

        [SerializeField] private float _healthDelta = 10f;
        [SerializeField] private float _maxHealthValue = 100f;
        [SerializeField] private float _maxHealthDelta = 10f;
        [SerializeField] private float _speedDelta = 1f;
        [SerializeField] private float _speedValue = 5f;

        [Button]
        private void Damage()
        {
            ApplyDamage(_healthDelta);
        }

        [Button]
        private void DirectDamage()
        {
            ApplyDirectDamage(_healthDelta);
        }

        [Button]
        private void Heal()
        {
            ChangeCurrentHealth(_healthDelta);
        }

        [Button]
        private void RestoreHealth()
        {
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    queue.EnqueueRestore(entity);
                }
            });
        }

        [Button]
        private void Kill()
        {
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    queue.EnqueueKill(entity);
                }
            });
        }

        [Button]
        private void SetMaxHealth()
        {
            var maxHealth = Mathf.Max(_minHealthValue, _maxHealthValue);
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    queue.EnqueueSetMax(entity, maxHealth);
                }
            });
        }

        [Button]
        private void IncreaseMaxHealth()
        {
            ChangeMaxHealth(_maxHealthDelta);
        }

        [Button]
        private void DecreaseMaxHealth()
        {
            ChangeMaxHealth(-_maxHealthDelta);
        }

        [Button]
        private void IncreaseSpeed()
        {
            ChangeSpeed(_speedDelta);
        }

        [Button]
        private void DecreaseSpeed()
        {
            ChangeSpeed(-_speedDelta);
        }

        [Button]
        private void SetSpeed()
        {
            var speedValue = Mathf.Max(_minSpeedValue, _speedValue);

            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, PlayerMoveSpeed>>().Entities())
                {
                    ref var speed = ref entity.Ref<PlayerMoveSpeed>();
                    speed._value = speedValue;
                }
            }
        }

        [Button]
        private void PrintInventoryStats()
        {
            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health, Armor, InventoryComponent, InventoryEquipmentStats>>().Entities())
                {
                    ref readonly var health = ref entity.Read<Health>();
                    ref readonly var armor = ref entity.Read<Armor>();
                    ref readonly var inventory = ref entity.Read<InventoryComponent>();
                    ref readonly var equipmentStats = ref entity.Read<InventoryEquipmentStats>();
                    var currentStats = InventoryOperations.CalculateEquipmentStats(inventory._state);
                    Debug.Log($"Inventory stats. Health: {health._currentValue.Value}/{health._maxValue.Value}. Armor: {armor._value.Value}. AppliedMaxHealthBonus: {equipmentStats._appliedStats._maxHealthBonus}. CurrentMaxHealthBonus: {currentStats._maxHealthBonus}. CurrentArmorBonus: {currentStats._armorBonus}.");
                }
            }
        }

        private void ApplyDamage(float damage)
        {
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    queue.EnqueueDamage(entity, damage);
                }
            });
        }

        private void ApplyDirectDamage(float damage)
        {
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    queue.EnqueueDirectDamage(entity, damage);
                }
            });
        }

        private void ChangeCurrentHealth(float delta)
        {
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    if (delta >= 0f)
                    {
                        queue.EnqueueHeal(entity, delta);
                    }

                    if (delta < 0f)
                    {
                        queue.EnqueueDirectDamage(entity, -delta);
                    }
                }
            });
        }

        private void ChangeMaxHealth(float delta)
        {
            EnqueueHealthCommand(queue =>
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    queue.EnqueueChangeMax(entity, delta);
                }
            });
        }

        private void ChangeSpeed(float delta)
        {
            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, PlayerMoveSpeed>>().Entities())
                {
                    ref var speed = ref entity.Ref<PlayerMoveSpeed>();
                    speed._value = Mathf.Max(_minSpeedValue, speed._value + delta);
                }
            }
        }

        private void EnqueueHealthCommand(Action<HealthCommandQueue> enqueue)
        {
            if (CanDebug())
            {
                ref var queue = ref GameWorld.GetResource<HealthCommandQueue>();
                enqueue(queue);
            }
        }

        private bool CanDebug()
        {
            var canDebug = Application.isPlaying;

            if (!canDebug)
            {
                Debug.LogError(_playModeError);
            }

            return canDebug;
        }
    }
}