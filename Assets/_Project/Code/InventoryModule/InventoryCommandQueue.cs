using System.Collections.Generic;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule
{
    public sealed class InventoryCommandQueue : IResource
    {
        private readonly Queue<InventoryCommand> _commands = new Queue<InventoryCommand>();

        public int Count => _commands.Count;

        public void Enqueue(InventoryCommand command)
        {
            _commands.Enqueue(command);
        }

        public InventoryCommand Dequeue()
        {
            return _commands.Dequeue();
        }
    }
}
