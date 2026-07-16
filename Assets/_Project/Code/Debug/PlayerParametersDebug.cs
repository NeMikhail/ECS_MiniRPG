using ECSMiniRPG.GameplayModule.Components;
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
            ChangeCurrentHealth(-_healthDelta);
        }

        [Button]
        private void Heal()
        {
            ChangeCurrentHealth(_healthDelta);
        }

        [Button]
        private void RestoreHealth()
        {
            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    ref var health = ref entity.Ref<Health>();
                    health.SetCurrentValue(health._maxValue.Value);
                }
            }
        }

        [Button]
        private void Kill()
        {
            SetCurrentHealth(_minHealthValue);
        }

        [Button]
        private void SetMaxHealth()
        {
            var maxHealth = Mathf.Max(_minHealthValue, _maxHealthValue);

            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    ref var health = ref entity.Ref<Health>();
                    health._maxValue.Value = maxHealth;
                    health.SetCurrentValue(health._currentValue.Value);
                }
            }
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

        private void ChangeCurrentHealth(float delta)
        {
            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    ref var health = ref entity.Ref<Health>();
                    health.SetCurrentValue(health._currentValue.Value + delta);
                }
            }
        }

        private void SetCurrentHealth(float value)
        {
            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    ref var health = ref entity.Ref<Health>();
                    health.SetCurrentValue(value);
                }
            }
        }

        private void ChangeMaxHealth(float delta)
        {
            if (CanDebug())
            {
                foreach (var entity in GameWorld.Query<All<PlayerTag, Health>>().Entities())
                {
                    ref var health = ref entity.Ref<Health>();
                    health._maxValue.Value = Mathf.Max(_minHealthValue, health._maxValue.Value + delta);
                    health.SetCurrentValue(health._currentValue.Value);
                }
            }
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