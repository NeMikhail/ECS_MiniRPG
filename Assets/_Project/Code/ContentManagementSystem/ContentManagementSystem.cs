using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ECSMiniRPG.ContentManagement
{
    public sealed class ContentManagementSystem : IResource
    {
        public AsyncOperationHandle<GameObject> InstantiateAsync(string key, Vector3 position, Quaternion rotation)
        {
            return Addressables.InstantiateAsync(key, position, rotation);
        }

        public void Release(AsyncOperationHandle<GameObject> handle)
        {
            Addressables.Release(handle);
        }

        public void ReleaseInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
        }
    }
}
