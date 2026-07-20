using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobAttackRangedHitscanFileName, menuName = ConfigAssetNames._mobAttackRangedHitscanMenuName)]
    public sealed class Attack_Mob_RangedHitscanBehaviourConfig : MobAttackBehaviourConfig
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 12f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private LayerMask _hitMask = Physics.DefaultRaycastLayers;
        [SerializeField] private Vector3 _originOffset = Vector3.zero;

        public float Damage => _damage;
        public float AttackRange => _attackRange;
        public float AttackCooldown => _attackCooldown;
        public LayerMask HitMask => _hitMask;
        public Vector3 OriginOffset => _originOffset;

        public override void Execute(MobAttackContext context, ref MobAttackState state)
        {
            if (context._hasTarget && context.IsTargetInRange(_attackRange) && TryStartCooldown(context, ref state, _attackCooldown))
            {
                context.DamageTarget(_damage);
            }
        }
    }
}


