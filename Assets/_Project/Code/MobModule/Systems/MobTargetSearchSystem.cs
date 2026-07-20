using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.LoggerModule;
using ECSMiniRPG.MobModule.Components;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Systems
{
    public struct MobTargetSearchSystem : ISystem
    {
        public void Update()
        {
            foreach (var mob in GameWorld.Query<All<MobTag, MobTarget, LongActionTransformRef>>().Entities())
            {
                ref var target = ref mob.Ref<MobTarget>();
                ref readonly var mobTransform = ref mob.Read<LongActionTransformRef>();

                FindNearestPlayerTarget(mob, ref target, mobTransform);
            }
        }

        private void FindNearestPlayerTarget(GameWorld.Entity mob, ref MobTarget target, LongActionTransformRef mobTransform)
        {
            var hasTarget = false;
            var bestTarget = default(GameWorld.Entity);
            var bestSqrDistance = float.MaxValue;

            foreach (var player in GameWorld.Query<All<PlayerTag, Health, LongActionTransformRef>>().Entities())
            {
                ref readonly var health = ref player.Read<Health>();
                ref readonly var playerTransform = ref player.Read<LongActionTransformRef>();
                var sqrDistance = (playerTransform._value.position - mobTransform._value.position).sqrMagnitude;

                LoggerSystem.Info(EcsModuleType.Mob, $"Target candidate. mob={mob} player={player} hp={health._currentValue.Value}/{health._maxValue.Value} distance={Mathf.Sqrt(sqrDistance)}");

                if (health._currentValue.Value > 0f && sqrDistance < bestSqrDistance)
                {
                    bestTarget = player;
                    bestSqrDistance = sqrDistance;
                    hasTarget = true;
                }
            }

            if (hasTarget)
            {
                var hasChanged = !target.HasSameValue(bestTarget);
                target.Set(bestTarget);

                if (hasChanged)
                {
                    LoggerSystem.Debug(EcsModuleType.Mob, $"Target selected. mob={mob} target={bestTarget} distance={Mathf.Sqrt(bestSqrDistance)}");
                }
            }

            if (!hasTarget && target._hasValue)
            {
                target.Clear();
                LoggerSystem.Debug(EcsModuleType.Mob, $"Target cleared. mob={mob}");
            }
        }
    }
}