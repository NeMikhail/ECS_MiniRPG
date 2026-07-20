using System.Collections.Generic;
using ECSMiniRPG.GUIModule.Views;
using ECSMiniRPG.MobModule.Configs;
using UnityEngine;
using UnityEngine.AI;

namespace ECSMiniRPG.MobModule.Views
{
    public class MobView : MonoBehaviour
    {
        private static readonly List<Transform> _emptyPatrolPoints = new List<Transform>();

        [SerializeField] private MobConfig _config;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private Camera _healthBarCamera;

        private MobPatrolRoute _patrolRoute;

        public MobConfig Config => _config;
        public NavMeshAgent Agent => GetAgent();
        public HealthBarView HealthBarView => GetHealthBarView();
        public Camera HealthBarCamera => GetHealthBarCamera();
        public IReadOnlyList<Transform> PatrolPoints => GetPatrolPoints();

        public void SetConfig(MobConfig config)
        {
            _config = config;
        }

        private void Awake()
        {
            _patrolRoute = GetComponent<MobPatrolRoute>();
            GetAgent();
            GetHealthBarView();
        }

        private NavMeshAgent GetAgent()
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }

            return _agent;
        }

        private HealthBarView GetHealthBarView()
        {
            if (_healthBarView == null)
            {
                _healthBarView = GetComponentInChildren<HealthBarView>(true);
            }

            return _healthBarView;
        }

        private Camera GetHealthBarCamera()
        {
            if (_healthBarCamera == null)
            {
                _healthBarCamera = Camera.main;
            }

            return _healthBarCamera;
        }

        private IReadOnlyList<Transform> GetPatrolPoints()
        {
            IReadOnlyList<Transform> points = _emptyPatrolPoints;

            if (_patrolRoute != null)
            {
                points = _patrolRoute.Points;
            }

            return points;
        }
    }
}