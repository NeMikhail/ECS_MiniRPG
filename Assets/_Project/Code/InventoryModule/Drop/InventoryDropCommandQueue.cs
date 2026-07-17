using System.Collections.Generic;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Drop
{
    public sealed class InventoryDropCommandQueue : IResource
    {
        private readonly Queue<InventoryDropCommand> _commands = new Queue<InventoryDropCommand>();

        public int Count => _commands.Count;

        public void Enqueue(GameWorld.Entity actor, InventoryDragData sourceData, int count, InventoryDropSourceType sourceType)
        {
            _commands.Enqueue(new InventoryDropCommand(actor, sourceData, count, sourceType));
        }

        public InventoryDropCommand Dequeue()
        {
            return _commands.Dequeue();
        }
    }
}
