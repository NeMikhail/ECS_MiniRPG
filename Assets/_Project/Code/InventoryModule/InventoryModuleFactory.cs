using ECSMiniRPG.Core;
using ECSMiniRPG.InventoryModule.Drop;
using ECSMiniRPG.InventoryModule.Pickup;
using ECSMiniRPG.InventoryModule.Systems;
using ECSMiniRPG.InventoryModule.Tooling;

namespace ECSMiniRPG.InventoryModule
{
    public sealed class InventoryModuleFactory : IEcsModuleFactory
    {
        private static readonly short _initSystemOrder = 10;
        private static readonly short _commandSystemOrder = 20;
        private static readonly short _pickupInputSystemOrder = 15;
        private static readonly short _pickupSystemOrder = 26;
        private static readonly short _toolCacheSystemOrder = 25;
        private static readonly short _dropSystemOrder = 27;
        private static readonly short _equipmentStatsSystemOrder = 30;

        public void RegisterResources()
        {
            GameWorld.SetResource(new InventoryCommandQueue());
            GameWorld.SetResource(new InventoryPickupCommandQueue());
            GameWorld.SetResource(new InventoryPickupRuntimeData());
            GameWorld.SetResource(new InventoryDropCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new InventoryInitSystem(), _initSystemOrder);
            GameSystems.Add(new InventoryPickupInputSystem(), _pickupInputSystemOrder);
            GameSystems.Add(new InventoryCommandSystem(), _commandSystemOrder);
            GameSystems.Add(new InventoryToolCacheSystem(), _toolCacheSystemOrder);
            GameSystems.Add(new InventoryPickupSystem(), _pickupSystemOrder);
            GameSystems.Add(new InventoryDropSystem(), _dropSystemOrder);
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
