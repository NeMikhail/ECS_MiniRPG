using ECSMiniRPG.InventoryModule.Items;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Loot
{
    public readonly struct LocationLootSpawnCommand
    {
        public readonly ItemPreset _item;
        public readonly int _count;
        public readonly Vector3 _position;
        public readonly Quaternion _rotation;

        public LocationLootSpawnCommand(ItemPreset item, int count, Vector3 position, Quaternion rotation)
        {
            _item = item;
            _count = count;
            _position = position;
            _rotation = rotation;
        }
    }
}