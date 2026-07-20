using System.Collections.Generic;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Views
{
    public sealed class MobPatrolRoute : MonoBehaviour
    {
        [SerializeField] private List<Transform> _points = new List<Transform>();

        public IReadOnlyList<Transform> Points => _points;
    }
}
