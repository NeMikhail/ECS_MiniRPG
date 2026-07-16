using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Configs
{
    [CreateAssetMenu(fileName = "MaxHealthItemStatModifierConfig", menuName = "ECSMiniRPG/Inventory/Stat Modifiers/Max Health")]
    public sealed class MaxHealthItemStatModifierConfig : ItemStatModifierConfig
    {
        [SerializeField] private float _value = 10f;

        public override void Append(ref InventoryStatSnapshot snapshot)
        {
            snapshot._maxHealthBonus += _value;
        }
    }
}
