using ECSMiniRPG.GameplayModule.Components;

namespace ECSMiniRPG.InventoryModule.Items
{
    public interface IConsumable : IItem
    {
        bool CanUse(Health health);
        void Use(ref Health health);
    }
}
