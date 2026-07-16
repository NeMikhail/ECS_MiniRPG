using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Components
{
    public struct InventoryComponent : IComponent
    {
        public InventoryState _state;
        public bool _isInitialized;
        public bool _isDirty;

        public static InventoryComponent Create()
        {
            return new InventoryComponent
            {
                _state = new InventoryState()
            };
        }
    }
}
