using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ECSMiniRPG.MobModule.Components
{
    public struct MobContentRef : IComponent
    {
        public GameObject _instance;
        public bool _isAddressableInstance;

        public void OnDelete<TWorld>(World<TWorld>.Entity self, HookReason reason) where TWorld : struct, IWorldType
        {
            if (_instance != null && _isAddressableInstance)
            {
                Addressables.ReleaseInstance(_instance);
            }

            if (_instance != null && !_isAddressableInstance)
            {
                Object.Destroy(_instance);
            }
        }
    }
}
