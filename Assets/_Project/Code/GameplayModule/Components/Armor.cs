using FFS.Libraries.StaticEcs;
using R3;
using UnityEngine;

namespace ECSMiniRPG.GameplayModule.Components
{
    public struct Armor : IComponent
    {
        public ReactiveProperty<float> _value;

        public static Armor Create(float value)
        {
            return new Armor
            {
                _value = new ReactiveProperty<float>(Mathf.Max(0f, value))
            };
        }

        public void SetValue(float value)
        {
            _value.Value = Mathf.Max(0f, value);
        }

        public void Dispose()
        {
            _value.Dispose();
        }

        public void OnDelete<TWorld>(World<TWorld>.Entity self, HookReason reason) where TWorld : struct, IWorldType
        {
            Dispose();
        }
    }
}

