using ECSMiniRPG.GameplayModule.Components;

namespace ECSMiniRPG.InventoryModule.Items
{
    public interface IConsumable : IItem
    {
        float HealValue { get; }
        bool CanUse(Health health);
    }
}