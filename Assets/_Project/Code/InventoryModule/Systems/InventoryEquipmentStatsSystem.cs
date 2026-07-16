using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.InventoryModule.Runtime;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Systems
{
    public struct InventoryEquipmentStatsSystem : ISystem
    {
        public void Update()
        {
            foreach (var entity in GameWorld.Query<All<PlayerTag, InventoryComponent, InventoryEquipmentStats, Health, Armor>>().Entities())
            {
                ref var inventory = ref entity.Ref<InventoryComponent>();
                ref var equipmentStats = ref entity.Ref<InventoryEquipmentStats>();
                ref var health = ref entity.Ref<Health>();
                ref var armor = ref entity.Ref<Armor>();
                var currentStats = InventoryOperations.CalculateEquipmentStats(inventory._state);
                var maxHealthDelta = currentStats._maxHealthBonus - equipmentStats._appliedStats._maxHealthBonus;
                var hasMaxHealthChanged = !Mathf.Approximately(maxHealthDelta, 0f);
                var hasArmorChanged = !Mathf.Approximately(currentStats._armorBonus, equipmentStats._appliedStats._armorBonus);

                if (hasMaxHealthChanged)
                {
                    health._maxValue.Value = Mathf.Max(0f, health._maxValue.Value + maxHealthDelta);
                    health.SetCurrentValue(health._currentValue.Value);
                }

                if (hasArmorChanged)
                {
                    armor.SetValue(currentStats._armorBonus);
                }

                if (hasMaxHealthChanged || hasArmorChanged || inventory._isDirty)
                {
                    equipmentStats._appliedStats = currentStats;
                    inventory._isDirty = false;
                }
            }
        }
    }
}

