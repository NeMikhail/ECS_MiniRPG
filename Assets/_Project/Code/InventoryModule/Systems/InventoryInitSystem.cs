using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.InventoryModule.Runtime;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Systems
{
    public struct InventoryInitSystem : ISystem
    {
        public void Update()
        {
            ref var gameConfigs = ref GameWorld.GetResource<GameConfigs>();

            foreach (var entity in GameWorld.Query<All<PlayerTag, InventoryComponent>>().Entities())
            {
                ref var inventory = ref entity.Ref<InventoryComponent>();

                if (!inventory._isInitialized)
                {
                    InventoryOperations.Initialize(inventory._state, gameConfigs.InventoryConfig.SlotsCount);
                    InventoryOperations.AddStartItems(inventory._state, gameConfigs.InventoryConfig);
                    inventory._isInitialized = true;
                    inventory._isDirty = true;
                }
            }
        }
    }
}
