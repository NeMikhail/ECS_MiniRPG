using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.LongActions;
using ECSMiniRPG.GameplayModule.LongActions.Systems;
using ECSMiniRPG.GameplayModule.Systems;

namespace ECSMiniRPG.GameplayModule
{
    public sealed class GameplayModuleFactory : IEcsModuleFactory
    {
        private static readonly short _healthCommandSystemOrder = 40;
        private static readonly short _longActionSystemOrder = 45;

        public void RegisterResources()
        {
            GameWorld.SetResource(new LongActionCommandQueue());
            GameWorld.SetResource(new HealthCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new HealthCommandSystem(), _healthCommandSystemOrder);
            GameSystems.Add(new LongActionSystem(), _longActionSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }
    }
}