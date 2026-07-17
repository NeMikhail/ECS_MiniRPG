using System.Collections.Generic;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Tooling
{
    public struct InventoryToolCacheSystem : ISystem
    {
        public void Update()
        {
            foreach (var entity in GameWorld.Query<All<PlayerTag, InventoryComponent>>().Entities())
            {
                UpdateCache(entity);
            }
        }

        private void UpdateCache(GameWorld.Entity entity)
        {
            ref var inventory = ref entity.Ref<InventoryComponent>();

            if (ShouldUpdateCache(entity, inventory))
            {
                var toolTypes = BuildToolTypes(inventory._state);
                entity.Set(new InventoryToolCache
                {
                    _toolTypes = toolTypes
                });
            }
        }

        private bool ShouldUpdateCache(GameWorld.Entity entity, InventoryComponent inventory)
        {
            var shouldUpdate = !entity.Has<InventoryToolCache>();

            if (!shouldUpdate)
            {
                shouldUpdate = inventory._isDirty;
            }

            return shouldUpdate;
        }

        private HashSet<ToolTypeConfig> BuildToolTypes(InventoryState state)
        {
            var toolTypes = new HashSet<ToolTypeConfig>();

            for (var i = 0; i < state.InventorySlots.Count; i++)
            {
                AddToolType(toolTypes, state.InventorySlots[i].Item);
            }

            for (var i = 0; i < state.EquipmentSlots.Count; i++)
            {
                AddToolType(toolTypes, state.EquipmentSlots[i].Item);
            }

            return toolTypes;
        }

        private void AddToolType(HashSet<ToolTypeConfig> toolTypes, InventoryItemInstance item)
        {
            if (item != null && item.Preset != null)
            {
                var toolItem = item.Preset as ToolItemConfig;

                if (toolItem != null && toolItem.ToolType != null)
                {
                    toolTypes.Add(toolItem.ToolType);
                }
            }
        }
    }
}
