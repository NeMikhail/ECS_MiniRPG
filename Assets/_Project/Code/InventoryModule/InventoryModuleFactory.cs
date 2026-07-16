using ECSMiniRPG.Core;
using ECSMiniRPG.InventoryModule.Systems;

namespace ECSMiniRPG.InventoryModule
{
    public sealed class InventoryModuleFactory : IEcsModuleFactory
    {
        private static readonly short _initSystemOrder = 10;
        private static readonly short _commandSystemOrder = 20;
        private static readonly short _equipmentStatsSystemOrder = 30;

        public void RegisterResources()
        {
            GameWorld.SetResource(new InventoryCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new InventoryInitSystem(), _initSystemOrder);
            GameSystems.Add(new InventoryCommandSystem(), _commandSystemOrder);
            GameSystems.Add(new InventoryEquipmentStatsSystem(), _equipmentStatsSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }
    }
}
