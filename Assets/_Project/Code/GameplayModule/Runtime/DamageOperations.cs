using UnityEngine;

namespace ECSMiniRPG.GameplayModule.Runtime
{
    public static class DamageOperations
    {
        private static readonly float _minValue = 0f;

        public static float ClampDamage(float damage)
        {
            return Mathf.Max(_minValue, damage);
        }

        public static float CalculateMitigatedDamage(float damage, float armor)
        {
            var clampedArmor = Mathf.Max(_minValue, armor);
            var damageMultiplier = 1f / (1f + clampedArmor);
            return ClampDamage(damage) * damageMultiplier;
        }
    }
}