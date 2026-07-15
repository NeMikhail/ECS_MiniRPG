using System.Collections.Generic;
using ECSMiniRPG.PlayerModule.Configs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ECSMiniRPG.LocationModule
{
    public sealed class LocationView : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<string, Transform> _spawnPoints = new Dictionary<string, Transform>();

        public bool TryGetSpawnPose(string spawnPointId, out PlayerSpawnPose spawnPose)
        {
            var hasSpawnPose = false;
            spawnPose = PlayerSpawnPose.Identity;

            if (string.IsNullOrEmpty(spawnPointId))
            {
                Debug.LogError("Spawn point id is empty.");
            }

            if (!string.IsNullOrEmpty(spawnPointId) && !_spawnPoints.ContainsKey(spawnPointId))
            {
                Debug.LogError($"Spawn point with id '{spawnPointId}' is not found in LocationView.");
            }

            if (!string.IsNullOrEmpty(spawnPointId) && _spawnPoints.ContainsKey(spawnPointId))
            {
                hasSpawnPose = TryCreateSpawnPose(spawnPointId, out spawnPose);
            }

            return hasSpawnPose;
        }

        private bool TryCreateSpawnPose(string spawnPointId, out PlayerSpawnPose spawnPose)
        {
            var hasSpawnPose = false;
            var spawnPoint = _spawnPoints[spawnPointId];
            spawnPose = PlayerSpawnPose.Identity;

            if (spawnPoint == null)
            {
                Debug.LogError($"Spawn point with id '{spawnPointId}' has no Transform reference.");
            }

            if (spawnPoint != null)
            {
                spawnPose = new PlayerSpawnPose(spawnPoint.position, spawnPoint.rotation);
                hasSpawnPose = true;
            }

            return hasSpawnPose;
        }
    }
}
