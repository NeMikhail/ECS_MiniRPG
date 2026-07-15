using ECSMiniRPG.ContentManagement;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Components
{
    public struct PlayerContentRef : IComponent
    {
        public GameObject _instance;

        public void OnDelete<TWorld>(World<TWorld>.Entity self, HookReason reason) where TWorld : struct, IWorldType
        {
            ref var contentManagementSystem = ref World<TWorld>.GetResource<ContentManagementSystem>();
            contentManagementSystem.ReleaseInstance(_instance);
        }
    }
}
