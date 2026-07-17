using System.Collections.Generic;
using ECSMiniRPG.LocationModule.Resources;
using ECSMiniRPG.LocationModule.Resources.Views;
using ECSMiniRPG.PlayerModule.Configs;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.LocationModule
{
    public sealed class LocationRuntimeData : IResource
    {
        private readonly List<LocationResourceSpawnPointRuntimeData> _resourceSpawnPoints = new List<LocationResourceSpawnPointRuntimeData>();
        private LocationView _locationView;

        public bool HasLocationView => _locationView != null;
        public IReadOnlyList<LocationResourceSpawnPointRuntimeData> ResourceSpawnPoints => _resourceSpawnPoints;

        public void SetLocationView(LocationView locationView)
        {
            _locationView = locationView;
            BuildResourceSpawnPoints(locationView);
        }

        public bool TryGetSpawnPose(string spawnPointId, out PlayerSpawnPose spawnPose)
        {
            var hasSpawnPose = false;
            spawnPose = PlayerSpawnPose.Identity;

            if (_locationView == null)
            {
                Debug.LogError("LocationView is not assigned in location runtime data.");
            }

            if (_locationView != null)
            {
                hasSpawnPose = _locationView.TryGetSpawnPose(spawnPointId, out spawnPose);
            }

            return hasSpawnPose;
        }

        private void BuildResourceSpawnPoints(LocationView locationView)
        {
            _resourceSpawnPoints.Clear();

            for (var i = 0; i < locationView.ResourceSpawnPoints.Count; i++)
            {
                AddResourceSpawnPoint(locationView.ResourceSpawnPoints[i]);
            }
        }

        private void AddResourceSpawnPoint(LocationResourceSpawnPointView spawnPointView)
        {
            if (spawnPointView != null && spawnPointView.Config != null)
            {
                _resourceSpawnPoints.Add(new LocationResourceSpawnPointRuntimeData(spawnPointView));
            }
        }
    }
}