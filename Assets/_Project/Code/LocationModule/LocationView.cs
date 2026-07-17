using System.Collections.Generic;
using ECSMiniRPG.Core;
using ECSMiniRPG.LocationModule.Resources.Views;
using ECSMiniRPG.PlayerModule.Configs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ECSMiniRPG.LocationModule
{
    public sealed class LocationView : SerializedMonoBehaviour, IView
    {
        [OdinSerialize] private Dictionary<string, Transform> _spawnPoints = new Dictionary<string, Transform>();
        [SerializeField] private ResourceNodesSpawnView _resourceNodesSpawn;
        [SerializeField, HideInInspector] private List<LocationResourceSpawnPointView> _resourceSpawnPoints = new List<LocationResourceSpawnPointView>();

        public IReadOnlyList<LocationResourceSpawnPointView> ResourceSpawnPoints => GetResourceSpawnPoints();

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


        private IReadOnlyList<LocationResourceSpawnPointView> GetResourceSpawnPoints()
        {
            IReadOnlyList<LocationResourceSpawnPointView> resourceSpawnPoints = _resourceSpawnPoints;

            if (_resourceNodesSpawn != null)
            {
                resourceSpawnPoints = _resourceNodesSpawn.ResourceSpawnPoints;
            }

            return resourceSpawnPoints;
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
