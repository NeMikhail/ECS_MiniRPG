using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.GUIModule.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GUIModule.Systems
{
    public struct HealthBarBindingSystem : ISystem
    {
        public void Update()
        {
            foreach (var entity in GameWorld.Query<All<Health, HealthBarViewRef>>().Entities())
            {
                if (entity.Has<HealthBarBoundTag>())
                {
                    continue;
                }

                ref readonly var health = ref entity.Read<Health>();
                ref readonly var healthBarViewRef = ref entity.Read<HealthBarViewRef>();

                if (healthBarViewRef._value != null)
                {
                    healthBarViewRef._value.Bind(health);
                    entity.Set<HealthBarBoundTag>();
                }
            }
        }
    }
}