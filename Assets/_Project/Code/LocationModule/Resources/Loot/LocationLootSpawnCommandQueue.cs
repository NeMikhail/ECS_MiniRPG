using System.Collections.Generic;
using ECSMiniRPG.InventoryModule.Items;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Loot
{
    public sealed class LocationLootSpawnCommandQueue : IResource
    {
        private readonly Queue<LocationLootSpawnCommand> _commands = new Queue<LocationLootSpawnCommand>();

        public int Count => _commands.Count;

        public void Enqueue(ItemPreset item, int count, Vector3 position, Quaternion rotation)
        {
            _commands.Enqueue(new LocationLootSpawnCommand(item, count, position, rotation));
        }

        public LocationLootSpawnCommand Dequeue()
        {
            return _commands.Dequeue();
        }
    }
}