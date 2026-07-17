using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Configs
{
    [CreateAssetMenu(fileName = "LocationResourceSpawnPointConfig", menuName = "ECSMiniRPG/Location/Resource Spawn Point")]
    public sealed class LocationResourceSpawnPointConfig : ScriptableObject
    {
        [SerializeField] private List<LocationResourceWeightedEntry> _resources = new List<LocationResourceWeightedEntry>();
        [SerializeField] private float _initialSpawnDelay;
        [SerializeField] private float _depletedRestoreDelay = 10f;
        [SerializeField] private float _resourceRefreshInterval = 60f;
        [SerializeField] private float _playerVisibilityRadius = 15f;

        public IReadOnlyList<LocationResourceWeightedEntry> Resources => _resources;
        public float InitialSpawnDelay => Mathf.Max(0f, _initialSpawnDelay);
        public float DepletedRestoreDelay => Mathf.Max(0f, _depletedRestoreDelay);
        public float ResourceRefreshInterval => Mathf.Max(0f, _resourceRefreshInterval);
        public float PlayerVisibilityRadius => Mathf.Max(0f, _playerVisibilityRadius);
    }

    [Serializable]
    public sealed class LocationResourceWeightedEntry
    {
        [SerializeField] private ResourceNodeConfig _resource;
        [SerializeField] private int _weight = 1;

        public ResourceNodeConfig Resource => _resource;
        public int Weight => Mathf.Max(0, _weight);
    }
}
