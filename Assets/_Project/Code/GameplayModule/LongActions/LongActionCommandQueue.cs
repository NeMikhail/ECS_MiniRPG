using System.Collections.Generic;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.LongActions
{
    public sealed class LongActionCommandQueue : IResource
    {
        private readonly Queue<LongActionCommand> _commands = new Queue<LongActionCommand>();

        public int Count => _commands.Count;

        public void EnqueueStart(
            GameWorld.Entity owner,
            GameWorld.Entity target,
            float duration,
            float targetRange,
            LongActionCancelPolicyConfig cancelPolicy,
            LongActionBehaviourConfig behaviour
        )
        {
            _commands.Enqueue(LongActionCommand.Start(owner, target, duration, targetRange, cancelPolicy, behaviour));
        }

        public void EnqueueCancelOwner(GameWorld.Entity owner, LongActionCancelReason cancelReason)
        {
            _commands.Enqueue(LongActionCommand.CancelOwner(owner, cancelReason));
        }

        public LongActionCommand Dequeue()
        {
            return _commands.Dequeue();
        }
    }
}
