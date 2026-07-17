using ECSMiniRPG.GameplayModule.Components;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Items
{
    [CreateAssetMenu(fileName = "ConsumableItemPreset", menuName = "ECSMiniRPG/Inventory/Consumable Item")]
    public sealed class ConsumableItemPreset : ItemPreset, IConsumable
    {
        [SerializeField] private float _healValue = 25f;

        public float HealValue => _healValue;

        public bool CanUse(Health health)
        {
            return health._currentValue.Value < health._maxValue.Value;
        }
    }
}