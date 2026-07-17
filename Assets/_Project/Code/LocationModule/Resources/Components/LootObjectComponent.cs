using ECSMiniRPG.InventoryModule.Items;
using ECSMiniRPG.LocationModule.Resources.Views;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.LocationModule.Resources.Components
{
    public struct LootObjectComponent : IComponent
    {
        public ItemPreset _item;
        public int _count;
        public LootObjectView _view;
    }
}
