using System;
using ECSMiniRPG.InventoryModule.Items;

namespace ECSMiniRPG.InventoryModule
{
    [Serializable]
    public sealed class InventoryItemInstance
    {
        private long _instanceId;
        private ItemPreset _preset;
        private int _count;

        public InventoryItemInstance(long instanceId, ItemPreset preset, int count)
        {
            _instanceId = instanceId;
            _preset = preset;
            _count = count;
        }

        public long InstanceId => _instanceId;
        public ItemPreset Preset => _preset;
        public int Count => _count;

        public void SetCount(int count)
        {
            _count = count;
        }
    }
}
