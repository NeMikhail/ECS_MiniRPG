using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobMovementStandingFileName, menuName = ConfigAssetNames._mobMovementStandingMenuName)]
    public sealed class Movement_StandingBehaviourConfig : MobMovementBehaviourConfig
    {
        public override void Execute(MobMovementContext context, ref MobMovementState state)
        {
            if (context._view != null && context._view.Agent != null && context._view.Agent.enabled && context._view.Agent.isOnNavMesh)
            {
                context._view.Agent.ResetPath();
            }
        }
    }
}


