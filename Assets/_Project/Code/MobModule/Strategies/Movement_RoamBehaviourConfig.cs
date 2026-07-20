using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobMovementRoamFileName, menuName = ConfigAssetNames._mobMovementRoamMenuName)]
    public sealed class Movement_RoamBehaviourConfig : MobMovementBehaviourConfig
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private float _pointReachDistance = 0.25f;
        [SerializeField] private float _pointRefreshInterval = 2f;

        public float Speed => _speed;
        public float Radius => _radius;
        public float PointReachDistance => _pointReachDistance;
        public float PointRefreshInterval => _pointRefreshInterval;

        public override void Execute(MobMovementContext context, ref MobMovementState state)
        {
            if (context._time >= state._nextRoamTime || context.HasReachedDestination(state._roamDestination, _pointReachDistance))
            {
                state._roamDestination = GetNextDestination(context);
                state._nextRoamTime = context._time + _pointRefreshInterval;
            }

            context.SetDestination(state._roamDestination, _speed);
        }

        private Vector3 GetNextDestination(MobMovementContext context)
        {
            var destination = Vector3.zero;

            if (context._view != null)
            {
                var offset = Random.insideUnitCircle * _radius;
                destination = context._view.transform.position + new Vector3(offset.x, 0f, offset.y);
            }

            return destination;
        }
    }
}


