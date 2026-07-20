using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobMovementOrbitFileName, menuName = ConfigAssetNames._mobMovementOrbitMenuName)]
    public sealed class Movement_OrbitBehaviourConfig : MobMovementBehaviourConfig
    {
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _orbitRadius = 4f;
        [SerializeField] private float _angularSpeed = 90f;

        public float MoveSpeed => _moveSpeed;
        public float OrbitRadius => _orbitRadius;
        public float AngularSpeed => _angularSpeed;

        public override void Execute(MobMovementContext context, ref MobMovementState state)
        {
            if (context._hasTarget)
            {
                state._orbitAngle += _angularSpeed * context._deltaTime;
                var radians = state._orbitAngle * Mathf.Deg2Rad;
                var offset = new Vector3(Mathf.Cos(radians), 0f, Mathf.Sin(radians)) * _orbitRadius;
                var destination = context._targetTransform.position + offset;
                context.SetDestination(destination, _moveSpeed);
            }
        }
    }
}


