using ECSMiniRPG.GameplayModule;
using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.InventoryModule.Runtime;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Systems
{
    public struct InventoryCommandSystem : ISystem
    {
        public void Update()
        {
            ref var commandQueue = ref GameWorld.GetResource<InventoryCommandQueue>();

            while (commandQueue.Count > 0)
            {
                ProcessCommand(commandQueue.Dequeue());
            }
        }

        private void ProcessCommand(InventoryCommand command)
        {
            foreach (var entity in GameWorld.Query<All<PlayerTag, InventoryComponent, Health>>().Entities())
            {
                ref var inventory = ref entity.Ref<InventoryComponent>();
                ref var health = ref entity.Ref<Health>();
                var hasChanged = ProcessCommand(command, inventory._state, health, entity);

                if (hasChanged)
                {
                    inventory._isDirty = true;
                }
            }
        }

        private bool ProcessCommand(InventoryCommand command, InventoryState state, Health health, GameWorld.Entity entity)
        {
            var hasChanged = false;

            if (command._type == InventoryCommandType.UseOrEquipInventorySlot)
            {
                hasChanged = InventoryOperations.TryUseOrEquipFromInventory(state, command._sourceInventoryIndex, health, out var healValue);

                if (hasChanged && healValue > 0f)
                {
                    ref var healthCommandQueue = ref GameWorld.GetResource<HealthCommandQueue>();
                    healthCommandQueue.EnqueueHeal(entity, healValue);
                }
            }

            if (command._type == InventoryCommandType.UnequipEquipmentSlot)
            {
                hasChanged = InventoryOperations.TryUnequip(state, command._sourceEquipmentSlotId);
            }

            if (command._type == InventoryCommandType.MoveInventorySlot)
            {
                hasChanged = InventoryOperations.TryMoveInventorySlot(state, command._sourceInventoryIndex, command._targetInventoryIndex);
            }

            if (command._type == InventoryCommandType.MoveInventoryToEquipment)
            {
                hasChanged = InventoryOperations.TryMoveInventoryToEquipment(state, command._sourceInventoryIndex, command._targetEquipmentSlotId);
            }

            if (command._type == InventoryCommandType.MoveEquipmentToInventory)
            {
                hasChanged = InventoryOperations.TryMoveEquipmentToInventory(state, command._sourceEquipmentSlotId, command._targetInventoryIndex);
            }

            if (command._type == InventoryCommandType.MoveEquipmentSlot)
            {
                hasChanged = InventoryOperations.TryMoveEquipmentSlot(state, command._sourceEquipmentSlotId, command._targetEquipmentSlotId);
            }

            return hasChanged;
        }
    }
}
