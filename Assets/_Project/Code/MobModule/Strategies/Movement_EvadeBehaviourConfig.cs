using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobMovementEvadeFileName, menuName = ConfigAssetNames._mobMovementEvadeMenuName)]
    public sealed class Movement_EvadeBehaviourConfig : MobMovementBehaviourConfig
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _evadeDistance = 4f;
        [SerializeField] private float _stopDistance = 6f;

        public float Speed => _speed;
        public float EvadeDistance => _evadeDistance;
        public float StopDistance => _stopDistance;

        public override void Execute(MobMovementContext context, ref MobMovementState state)
        {
            if (context._hasTarget && context._view != null)
            {
                var offset = context._view.transform.position - context._targetTransform.position;

                if (offset.sqrMagnitude < _stopDistance * _stopDistance)
                {
                    var direction = GetDirection(context, offset);
                    var destination = context._view.transform.position + direction * _evadeDistance;
                    context.SetDestination(destination, _speed);
                }
            }
        }

        private Vector3 GetDirection(MobMovementContext context, Vector3 offset)
        {
            var direction = -context._view.transform.forward;

            if (offset.sqrMagnitude > Mathf.Epsilon)
            {
                direction = offset.normalized;
            }

            return direction;
        }
    }
}


