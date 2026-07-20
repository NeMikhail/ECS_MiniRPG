using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobAttackMeleeAoeFileName, menuName = ConfigAssetNames._mobAttackMeleeAoeMenuName)]
    public sealed class Attack_Mob_MeleeAOEBehaviourConfig : MobAttackBehaviourConfig
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private float _areaRadius = 2f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private LayerMask _hitMask = Physics.DefaultRaycastLayers;

        public float Damage => _damage;
        public float AttackRange => _attackRange;
        public float AreaRadius => _areaRadius;
        public float AttackCooldown => _attackCooldown;
        public LayerMask HitMask => _hitMask;

        public override void Execute(MobAttackContext context, ref MobAttackState state)
        {
            if (context._hasTarget && context.IsTargetInRange(_attackRange) && TryStartCooldown(context, ref state, _attackCooldown))
            {
                context.DamageTarget(_damage);
            }
        }
    }
}


