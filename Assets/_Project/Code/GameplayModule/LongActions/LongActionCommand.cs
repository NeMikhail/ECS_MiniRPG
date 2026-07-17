namespace ECSMiniRPG.GameplayModule.LongActions
{
    public readonly struct LongActionCommand
    {
        public readonly LongActionCommandType _type;
        public readonly GameWorld.Entity _owner;
        public readonly GameWorld.Entity _target;
        public readonly float _duration;
        public readonly float _targetRange;
        public readonly LongActionCancelPolicyConfig _cancelPolicy;
        public readonly LongActionBehaviourConfig _behaviour;
        public readonly LongActionCancelReason _cancelReason;

        public LongActionCommand(
            LongActionCommandType type,
            GameWorld.Entity owner,
            GameWorld.Entity target,
            float duration,
            float targetRange,
            LongActionCancelPolicyConfig cancelPolicy,
            LongActionBehaviourConfig behaviour,
            LongActionCancelReason cancelReason
        )
        {
            _type = type;
            _owner = owner;
            _target = target;
            _duration = duration;
            _targetRange = targetRange;
            _cancelPolicy = cancelPolicy;
            _behaviour = behaviour;
            _cancelReason = cancelReason;
        }

        public static LongActionCommand Start(
            GameWorld.Entity owner,
            GameWorld.Entity target,
            float duration,
            float targetRange,
            LongActionCancelPolicyConfig cancelPolicy,
            LongActionBehaviourConfig behaviour
        )
        {
            return new LongActionCommand(LongActionCommandType.Start, owner, target, duration, targetRange, cancelPolicy, behaviour, LongActionCancelReason.None);
        }

        public static LongActionCommand CancelOwner(GameWorld.Entity owner, LongActionCancelReason cancelReason)
        {
            return new LongActionCommand(LongActionCommandType.CancelOwner, owner, default, 0f, 0f, null, null, cancelReason);
        }
    }
}
