using ECSMiniRPG.LocationModule.Resources.Configs;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.LocationModule.Resources.Systems
{
    public readonly struct LocationResourceSpawnHandle
    {
        public readonly LocationResourceSpawnPointRuntimeData _spawnPoint;
        public readonly ResourceNodeConfig _resource;
        public readonly AsyncOperationHandle<GameObject> _handle;

        public LocationResourceSpawnHandle(LocationResourceSpawnPointRuntimeData spawnPoint, ResourceNodeConfig resource, AsyncOperationHandle<GameObject> handle)
        {
            _spawnPoint = spawnPoint;
            _resource = resource;
            _handle = handle;
        }
    }
}