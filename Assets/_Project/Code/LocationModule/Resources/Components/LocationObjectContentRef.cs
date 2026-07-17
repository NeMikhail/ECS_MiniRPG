using ECSMiniRPG.ContentManagement;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Components
{
    public struct LocationObjectContentRef : IComponent
    {
        public GameObject _instance;
        public bool _isAddressableInstance;

        public void OnDelete<TWorld>(World<TWorld>.Entity self, HookReason reason) where TWorld : struct, IWorldType
        {
            if (_instance != null && _isAddressableInstance && GameWorld.IsWorldInitialized)
            {
                ref var contentManagementSystem = ref GameWorld.GetResource<ContentManagementSystem>();
                contentManagementSystem.ReleaseInstance(_instance);
            }

            if (_instance != null && !_isAddressableInstance)
            {
                Object.Destroy(_instance);
            }
        }
    }
}