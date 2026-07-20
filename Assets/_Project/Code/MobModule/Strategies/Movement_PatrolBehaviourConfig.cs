using System.Collections.Generic;
using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobMovementPatrolFileName, menuName = ConfigAssetNames._mobMovementPatrolMenuName)]
    public sealed class Movement_PatrolBehaviourConfig : MobMovementBehaviourConfig
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _pointReachDistance = 0.25f;
        [SerializeField] private List<Vector3> _points = new List<Vector3>();

        public float Speed => _speed;
        public float PointReachDistance => _pointReachDistance;
        public IReadOnlyList<Vector3> Points => _points;

        public override void Execute(MobMovementContext context, ref MobMovementState state)
        {
            var destination = GetDestination(context, state);

            if (context.HasReachedDestination(destination, _pointReachDistance))
            {
                state._patrolPointIndex = GetNextPointIndex(context, state);
                destination = GetDestination(context, state);
            }

            context.SetDestination(destination, _speed);
        }

        private Vector3 GetDestination(MobMovementContext context, MobMovementState state)
        {
            var destination = Vector3.zero;

            if (context._view != null)
            {
                destination = context._view.transform.position;
            }

            if (context._view != null && context._view.PatrolPoints.Count != 0)
            {
                var pointIndex = state._patrolPointIndex % context._view.PatrolPoints.Count;
                destination = context._view.PatrolPoints[pointIndex].position;
            }

            if ((context._view == null || context._view.PatrolPoints.Count == 0) && _points.Count != 0)
            {
                var pointIndex = state._patrolPointIndex % _points.Count;
                destination = _points[pointIndex];
            }

            return destination;
        }

        private int GetNextPointIndex(MobMovementContext context, MobMovementState state)
        {
            var pointsCount = 0;
            var pointIndex = state._patrolPointIndex;

            if (context._view != null)
            {
                pointsCount = context._view.PatrolPoints.Count;
            }

            if (pointsCount == 0)
            {
                pointsCount = _points.Count;
            }

            if (pointsCount != 0)
            {
                pointIndex = (state._patrolPointIndex + 1) % pointsCount;
            }

            return pointIndex;
        }
    }
}


