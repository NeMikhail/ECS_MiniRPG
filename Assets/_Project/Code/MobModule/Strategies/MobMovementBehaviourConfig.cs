using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    public abstract class MobMovementBehaviourConfig : ScriptableObject
    {
        public abstract void Execute(MobMovementContext context, ref MobMovementState state);
    }
}
