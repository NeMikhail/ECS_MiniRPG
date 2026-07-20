using ECSMiniRPG.Core;
using ECSMiniRPG.LoggerModule;
using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobMovementPursuitFileName, menuName = ConfigAssetNames._mobMovementPursuitMenuName)]
    public sealed class Movement_PursuitBehaviourConfig : MobMovementBehaviourConfig
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _stopDistance = 1.5f;

        public float Speed => _speed;
        public float StopDistance => _stopDistance;

        public override void Execute(MobMovementContext context, ref MobMovementState state)
        {
            if (context._hasTarget)
            {
                var hasReached = context.HasReachedDestination(context._targetTransform.position, _stopDistance);
                LoggerSystem.Info(EcsModuleType.Mob, $"Pursuit request. mob={context._mobEntity} target={context._targetEntity} hasReached={hasReached} stopDistance={_stopDistance} speed={_speed}", context._view);

                if (!hasReached)
                {
                    context.SetDestination(context._targetTransform.position, _speed);
                }
            }

            if (!context._hasTarget)
            {
                LoggerSystem.Info(EcsModuleType.Mob, $"Pursuit skipped. mob={context._mobEntity} hasTarget=false", context._view);
            }
        }
    }
}