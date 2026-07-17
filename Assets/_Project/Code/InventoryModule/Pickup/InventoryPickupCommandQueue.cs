using System.Collections.Generic;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Pickup
{
    public sealed class InventoryPickupCommandQueue : IResource
    {
        private readonly Queue<InventoryPickupCommand> _commands = new Queue<InventoryPickupCommand>();

        public int Count => _commands.Count;

        public void Enqueue(GameWorld.Entity actor, GameWorld.Entity target, InventoryPickupSourceType sourceType)
        {
            _commands.Enqueue(new InventoryPickupCommand(actor, target, sourceType));
        }

        public InventoryPickupCommand Dequeue()
        {
            return _commands.Dequeue();
        }
    }
}
