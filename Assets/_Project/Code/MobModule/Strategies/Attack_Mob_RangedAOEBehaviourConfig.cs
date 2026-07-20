using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobAttackRangedAoeFileName, menuName = ConfigAssetNames._mobAttackRangedAoeMenuName)]
    public sealed class Attack_Mob_RangedAOEBehaviourConfig : MobAttackBehaviourConfig
    {
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 8f;
        [SerializeField] private float _areaRadius = 2f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _projectileSpeed = 12f;
        [SerializeField] private string _projectilePrefabKey = "";
        [SerializeField] private LayerMask _hitMask = Physics.DefaultRaycastLayers;
        [SerializeField] private Vector3 _originOffset = Vector3.zero;

        public float Damage => _damage;
        public float AttackRange => _attackRange;
        public float AreaRadius => _areaRadius;
        public float AttackCooldown => _attackCooldown;
        public float ProjectileSpeed => _projectileSpeed;
        public string ProjectilePrefabKey => _projectilePrefabKey;
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


