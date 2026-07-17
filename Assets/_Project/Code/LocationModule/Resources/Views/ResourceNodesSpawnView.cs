using System.Collections.Generic;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Views
{
    public sealed class ResourceNodesSpawnView : MonoBehaviour
    {
        [SerializeField] private List<LocationResourceSpawnPointView> _resourceSpawnPoints = new List<LocationResourceSpawnPointView>();

        private bool _isInitialized;

        public IReadOnlyList<LocationResourceSpawnPointView> ResourceSpawnPoints
        {
            get
            {
                EnsureInitialized();
                return _resourceSpawnPoints;
            }
        }

        public void Initialize()
        {
            RebuildResourceSpawnPoints();
            _isInitialized = true;
        }

        private void Awake()
        {
            Initialize();
        }

        private void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }

        private void RebuildResourceSpawnPoints()
        {
            _resourceSpawnPoints.Clear();
            GetComponentsInChildren(true, _resourceSpawnPoints);
        }

        private void OnTransformChildrenChanged()
        {
            _isInitialized = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                RebuildResourceSpawnPoints();
            }
        }
#endif
    }
}
