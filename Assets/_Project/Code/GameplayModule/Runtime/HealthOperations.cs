using ECSMiniRPG.GameplayModule.Components;
using UnityEngine;

namespace ECSMiniRPG.GameplayModule.Runtime
{
    public static class HealthOperations
    {
        private static readonly float _minValue = 0f;

        public static float ApplyDamage(ref Health health, float damage)
        {
            var previousValue = health._currentValue.Value;
            health.SetCurrentValue(previousValue - DamageOperations.ClampDamage(damage));
            return previousValue - health._currentValue.Value;
        }

        public static float Heal(ref Health health, float value)
        {
            var previousValue = health._currentValue.Value;
            health.SetCurrentValue(previousValue + Mathf.Max(_minValue, value));
            return health._currentValue.Value - previousValue;
        }

        public static void SetCurrent(ref Health health, float value)
        {
            health.SetCurrentValue(value);
        }

        public static void Restore(ref Health health)
        {
            health.SetCurrentValue(health._maxValue.Value);
        }

        public static void Kill(ref Health health)
        {
            health.SetCurrentValue(_minValue);
        }

        public static void SetMax(ref Health health, float value)
        {
            health._maxValue.Value = Mathf.Max(_minValue, value);
            health.SetCurrentValue(health._currentValue.Value);
        }

        public static void ChangeMax(ref Health health, float delta)
        {
            SetMax(ref health, health._maxValue.Value + delta);
        }
    }
}