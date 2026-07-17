using System.Collections.Generic;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InteractionModule
{
    public sealed class InteractionCommandQueue : IResource
    {
        private readonly Queue<GameWorld.Entity> _actors = new Queue<GameWorld.Entity>();

        public int Count => _actors.Count;

        public void Enqueue(GameWorld.Entity actor)
        {
            _actors.Enqueue(actor);
        }

        public GameWorld.Entity Dequeue()
        {
            return _actors.Dequeue();
        }
    }
}
