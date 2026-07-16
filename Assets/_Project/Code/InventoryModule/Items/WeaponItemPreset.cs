using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Items
{
    [CreateAssetMenu(fileName = "WeaponItemPreset", menuName = "ECSMiniRPG/Inventory/Weapon Item")]
    public sealed class WeaponItemPreset : EquipmentItemPreset, IWeapon
    {
    }
}
