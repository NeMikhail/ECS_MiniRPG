using ECSMiniRPG.Core;
using ECSMiniRPG.LoggerModule;
using ECSMiniRPG.MobModule.Components;
using ECSMiniRPG.MobModule.Views;
using UnityEngine;
using UnityEngine.AI;

namespace ECSMiniRPG.MobModule.Strategies
{
    public readonly struct MobMovementContext
    {
        public readonly GameWorld.Entity _mobEntity;
        public readonly MobView _view;
        public readonly GameWorld.Entity _targetEntity;
        public readonly Transform _targetTransform;
        public readonly bool _hasTarget;
        public readonly float _deltaTime;
        public readonly float _time;

        public MobMovementContext(
            GameWorld.Entity mobEntity,
            MobViewRef viewRef,
            MobTarget target,
            Transform targetTransform,
            float deltaTime,
            float time
        )
        {
            _mobEntity = mobEntity;
            _view = viewRef._value;
            _targetEntity = target._value;
            _targetTransform = targetTransform;
            _hasTarget = target._hasValue && targetTransform != null;
            _deltaTime = deltaTime;
            _time = time;
        }

        public bool HasReachedDestination(Vector3 destination, float distance)
        {
            var hasReached = false;

            if (_view != null)
            {
                hasReached = (destination - _view.transform.position).sqrMagnitude <= distance * distance;
            }

            LoggerSystem.Info(EcsModuleType.Mob, $"Movement reach check. mob={_mobEntity} hasReached={hasReached} destination={destination} distance={distance}", _view);
            return hasReached;
        }

        public void SetDestination(Vector3 destination, float speed)
        {
            if (_view != null && _view.Agent != null && _view.Agent.enabled && _view.Agent.isOnNavMesh)
            {
                if (_view.Agent.areaMask == 0)
                {
                    _view.Agent.areaMask = NavMesh.AllAreas;
                }

                _view.Agent.speed = speed;
                _view.Agent.SetDestination(destination);
                LoggerSystem.Info(EcsModuleType.Mob, $"Movement destination set. mob={_mobEntity} target={_targetEntity} destination={destination} speed={speed} hasPath={_view.Agent.hasPath} pathPending={_view.Agent.pathPending} remainingDistance={_view.Agent.remainingDistance} isStopped={_view.Agent.isStopped}", _view);
            }

            if (_view == null)
            {
                LoggerSystem.Warning(EcsModuleType.Mob, $"Movement destination failed. mob={_mobEntity} view is null");
            }

            if (_view != null && _view.Agent == null)
            {
                LoggerSystem.Warning(EcsModuleType.Mob, $"Movement destination failed. mob={_mobEntity} agent is null", _view);
            }

            if (_view != null && _view.Agent != null && (!_view.Agent.enabled || !_view.Agent.isOnNavMesh))
            {
                LoggerSystem.Warning(EcsModuleType.Mob, $"Movement destination failed. mob={_mobEntity} agentEnabled={_view.Agent.enabled} isOnNavMesh={_view.Agent.isOnNavMesh}", _view);
            }
        }
    }
}