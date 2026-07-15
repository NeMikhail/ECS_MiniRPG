using ECSMiniRPG.PlayerModule.Configs;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.LocationModule
{
    public sealed class LocationRuntimeData : IResource
    {
        private LocationView _locationView;

        public bool HasLocationView => _locationView != null;

        public void SetLocationView(LocationView locationView)
        {
            _locationView = locationView;
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
    }
}
