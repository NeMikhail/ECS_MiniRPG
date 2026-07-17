using ECSMiniRPG.Core;
using ECSMiniRPG.GameplayModule.Systems;

namespace ECSMiniRPG.GameplayModule
{
    public sealed class GameplayModuleFactory : IEcsModuleFactory
    {
        private static readonly short _healthCommandSystemOrder = 40;

        public void RegisterResources()
        {
            GameWorld.SetResource(new HealthCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new HealthCommandSystem(), _healthCommandSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }
    }
}