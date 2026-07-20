using ECSMiniRPG.Core;
using ECSMiniRPG.LoggerModule;
using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobAttackMeleeFileName, menuName = ConfigAssetNames._mobAttackMeleeMenuName)]
    public sealed class Attack_Mob_MeleeBehaviourConfig : MobAttackBehaviourConfig
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private float _attackCooldown = 1f;

        public float Damage => _damage;
        public float AttackRange => _attackRange;
        public float AttackCooldown => _attackCooldown;

        public override void Execute(MobAttackContext context, ref MobAttackState state)
        {
            var isInRange = context.IsTargetInRange(_attackRange);
            var canAttack = state.CanAttack(context._time);
            LoggerSystem.Info(EcsModuleType.Mob, $"Melee attack request. mob={context._mobEntity} target={context._targetEntity} hasTarget={context._hasTarget} inRange={isInRange} canAttack={canAttack} time={context._time} nextAttack={state._nextAttackTime} damage={_damage} range={_attackRange}", context._view);

            if (context._hasTarget && isInRange && TryStartCooldown(context, ref state, _attackCooldown))
            {
                LoggerSystem.Debug(EcsModuleType.Mob, $"Melee attack damage. mob={context._mobEntity} target={context._targetEntity} damage={_damage}", context._view);
                context.DamageTarget(_damage);
            }
        }
    }
}