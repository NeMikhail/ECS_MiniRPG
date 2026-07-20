using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    public abstract class MobAttackBehaviourConfig : ScriptableObject
    {
        public abstract void Execute(MobAttackContext context, ref MobAttackState state);

        protected bool TryStartCooldown(MobAttackContext context, ref MobAttackState state, float cooldown)
        {
            var hasStarted = false;

            if (state.CanAttack(context._time))
            {
                state.StartCooldown(context._time, cooldown);
                hasStarted = true;
            }

            return hasStarted;
        }
    }
}
