using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule.LongActions.Components
{
    public struct LongActionComponent : IComponent
    {
        public GameWorld.Entity _target;
        public float _duration;
        public float _elapsed;
        public float _targetRange;
        public LongActionCancelPolicyConfig _cancelPolicy;
        public LongActionBehaviourConfig _behaviour;
    }
}
