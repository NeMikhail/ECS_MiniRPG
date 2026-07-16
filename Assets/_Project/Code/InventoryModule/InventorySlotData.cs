using System;

namespace ECSMiniRPG.InventoryModule
{
    [Serializable]
    public sealed class InventorySlotData
    {
        private int _index;
        private InventoryItemInstance _item;

        public InventorySlotData(int index)
        {
            _index = index;
        }

        public int Index => _index;
        public InventoryItemInstance Item => _item;
        public bool HasItem => _item != null;

        public void SetItem(InventoryItemInstance item)
        {
            _item = item;
        }

        public void Clear()
        {
            _item = null;
        }
    }
}
