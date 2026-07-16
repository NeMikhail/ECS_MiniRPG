using FFS.Libraries.StaticEcs;
using R3;
using UnityEngine;

namespace ECSMiniRPG.GameplayModule.Components
{
    public struct Health : IComponent
    {
        public ReactiveProperty<float> _currentValue;
        public ReactiveProperty<float> _maxValue;

        public static Health Create(float maxValue)
        {
            var clampedMaxValue = Mathf.Max(0f, maxValue);

            return new Health
            {
                _currentValue = new ReactiveProperty<float>(clampedMaxValue),
                _maxValue = new ReactiveProperty<float>(clampedMaxValue)
            };
        }

        public void SetCurrentValue(float value)
        {
            _currentValue.Value = Mathf.Clamp(value, 0f, _maxValue.Value);
        }

        public void Dispose()
        {
            _currentValue.Dispose();
            _maxValue.Dispose();
        }

        public void OnDelete<TWorld>(World<TWorld>.Entity self, HookReason reason) where TWorld : struct, IWorldType
        {
            Dispose();
        }
    }
}
