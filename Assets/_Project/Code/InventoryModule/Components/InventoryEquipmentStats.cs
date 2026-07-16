using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Components
{
    public struct InventoryEquipmentStats : IComponent
    {
        public InventoryStatSnapshot _appliedStats;
    }
}
