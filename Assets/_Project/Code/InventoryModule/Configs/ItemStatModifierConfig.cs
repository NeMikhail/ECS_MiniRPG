using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Configs
{
    public abstract class ItemStatModifierConfig : ScriptableObject
    {
        public abstract void Append(ref InventoryStatSnapshot snapshot);
    }
}
