using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Components
{
    public struct MobMovementState : IComponent
    {
        public Vector3 _roamDestination;
        public float _nextRoamTime;
        public int _patrolPointIndex;
        public float _orbitAngle;
    }
}
