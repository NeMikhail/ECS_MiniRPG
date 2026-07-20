using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.MobModule.Components;
using ECSMiniRPG.MobModule.Strategies;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Systems
{
    public struct MobAttackSystem : ISystem
    {
        public void Update()
        {
            foreach (var mob in GameWorld.Query<All<MobTag, MobConfigRef, MobViewRef, MobTarget, MobAttackState>>().Entities())
            {
                ref readonly var configRef = ref mob.Read<MobConfigRef>();

                if (configRef._value != null && configRef._value.AttackBehaviour != null)
                {
                    ExecuteAttack(mob, configRef._value.AttackBehaviour);
                }
            }
        }

        private void ExecuteAttack(GameWorld.Entity mob, MobAttackBehaviourConfig attackBehaviour)
        {
            ref readonly var viewRef = ref mob.Read<MobViewRef>();
            ref readonly var target = ref mob.Read<MobTarget>();
            ref var state = ref mob.Ref<MobAttackState>();
            var targetTransform = GetTargetTransform(target);
            var context = new MobAttackContext(mob, viewRef, target, targetTransform, Time.time);
            attackBehaviour.Execute(context, ref state);
        }

        private Transform GetTargetTransform(MobTarget target)
        {
            Transform targetTransform = null;

            if (target._hasValue && target._value.IsNotDestroyed && target._value.Has<LongActionTransformRef>())
            {
                ref readonly var transformRef = ref target._value.Read<LongActionTransformRef>();
                targetTransform = transformRef._value;
            }

            return targetTransform;
        }
    }
}
