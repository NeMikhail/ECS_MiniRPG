using ECSMiniRPG.InventoryModule.Items;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.LocationModule.Resources.Loot
{
    public readonly struct LocationLootSpawnHandle
    {
        public readonly ItemPreset _item;
        public readonly int _count;
        public readonly AsyncOperationHandle<GameObject> _handle;

        public LocationLootSpawnHandle(ItemPreset item, int count, AsyncOperationHandle<GameObject> handle)
        {
            _item = item;
            _count = count;
            _handle = handle;
        }
    }
}