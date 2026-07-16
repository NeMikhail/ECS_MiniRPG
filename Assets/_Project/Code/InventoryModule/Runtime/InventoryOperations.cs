using ECSMiniRPG.GameplayModule.Components;
using ECSMiniRPG.InventoryModule.Configs;
using ECSMiniRPG.InventoryModule.Items;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Runtime
{
    public static class InventoryOperations
    {
        public static void Initialize(InventoryState state, int slotsCount)
        {
            state.Clear();

            for (var i = 0; i < slotsCount; i++)
            {
                state.InventorySlots.Add(new InventorySlotData(i));
            }

            AddEquipmentSlot(state, EquipmentSlotId.Head, EquipmentSlotType.Head);
            AddEquipmentSlot(state, EquipmentSlotId.Chest, EquipmentSlotType.Chest);
            AddEquipmentSlot(state, EquipmentSlotId.Shoulders, EquipmentSlotType.Shoulders);
            AddEquipmentSlot(state, EquipmentSlotId.Gloves, EquipmentSlotType.Gloves);
            AddEquipmentSlot(state, EquipmentSlotId.Pants, EquipmentSlotType.Pants);
            AddEquipmentSlot(state, EquipmentSlotId.Boots, EquipmentSlotType.Boots);
            AddEquipmentSlot(state, EquipmentSlotId.RingLeft, EquipmentSlotType.Ring);
            AddEquipmentSlot(state, EquipmentSlotId.RingRight, EquipmentSlotType.Ring);
            AddEquipmentSlot(state, EquipmentSlotId.Amulet, EquipmentSlotType.Amulet);
            AddEquipmentSlot(state, EquipmentSlotId.MainHand, EquipmentSlotType.MainHand);
            AddEquipmentSlot(state, EquipmentSlotId.OffHand, EquipmentSlotType.OffHand);
            AddEquipmentSlot(state, EquipmentSlotId.QuickConsumable1, EquipmentSlotType.QuickConsumable);
            AddEquipmentSlot(state, EquipmentSlotId.QuickConsumable2, EquipmentSlotType.QuickConsumable);
        }

        public static void AddStartItems(InventoryState state, InventoryConfig config)
        {
            for (var i = 0; i < config.StartItems.Count; i++)
            {
                AddItem(state, config.StartItems[i].Preset, config.StartItems[i].Count);
            }
        }

        public static bool AddItem(InventoryState state, ItemPreset preset, int count)
        {
            var hasAddedAll = true;
            var remainingCount = count;

            while (remainingCount > 0 && hasAddedAll)
            {
                var stackCount = remainingCount <= preset.MaxStack ? remainingCount : preset.MaxStack;
                var item = new InventoryItemInstance(state.GetNextInstanceId(), preset, stackCount);
                var hasAdded = TryAddItem(state, item);

                if (hasAdded)
                {
                    remainingCount -= stackCount;
                }

                if (!hasAdded)
                {
                    hasAddedAll = false;
                }
            }

            return hasAddedAll;
        }

        public static bool TryAddItem(InventoryState state, InventoryItemInstance item)
        {
            var hasAdded = false;

            if (item != null && !HasInstance(state, item.InstanceId))
            {
                hasAdded = TryAddToExistingStack(state, item);

                if (!hasAdded)
                {
                    var freeSlot = GetFreeInventorySlot(state);
                    if (freeSlot != null)
                    {
                        freeSlot.SetItem(item);
                        hasAdded = true;
                    }
                }
            }

            return hasAdded;
        }

        public static bool TryUseOrEquipFromInventory(InventoryState state, int inventorySlotIndex, ref Health health)
        {
            var hasChanged = TryEquipFromInventory(state, inventorySlotIndex);

            if (!hasChanged)
            {
                hasChanged = TryUseFromInventory(state, inventorySlotIndex, ref health);
            }

            return hasChanged;
        }

        public static bool TryEquipFromInventory(InventoryState state, int inventorySlotIndex)
        {
            var hasEquipped = false;
            var inventorySlot = GetInventorySlot(state, inventorySlotIndex);

            if (inventorySlot != null && inventorySlot.Item != null)
            {
                var equipmentItem = inventorySlot.Item.Preset as IEquipmentItem;

                if (equipmentItem != null)
                {
                    if (equipmentItem.SlotType == EquipmentSlotType.TwoHanded)
                    {
                        hasEquipped = TryEquipTwoHandedItem(state, inventorySlot);
                    }

                    if (equipmentItem.SlotType != EquipmentSlotType.TwoHanded)
                    {
                        hasEquipped = TryEquipNonTwoHandedItem(state, inventorySlot, equipmentItem);
                    }
                }
            }

            return hasEquipped;
        }

        public static bool TryUnequip(InventoryState state, EquipmentSlotId slotId)
        {
            var hasUnequipped = false;
            var equipmentSlot = GetEquipmentSlot(state, slotId);
            var freeSlot = GetFreeInventorySlot(state);

            if (equipmentSlot != null && equipmentSlot.Item != null && freeSlot != null)
            {
                freeSlot.SetItem(equipmentSlot.Item);
                equipmentSlot.Clear();
                hasUnequipped = true;
            }

            return hasUnequipped;
        }

        public static bool TryMoveInventorySlot(InventoryState state, int sourceIndex, int targetIndex)
        {
            var hasMoved = false;
            var sourceSlot = GetInventorySlot(state, sourceIndex);
            var targetSlot = GetInventorySlot(state, targetIndex);

            if (sourceSlot != null && targetSlot != null && sourceSlot != targetSlot)
            {
                SwapItems(sourceSlot, targetSlot);
                hasMoved = true;
            }

            return hasMoved;
        }

        public static bool TryMoveInventoryToEquipment(InventoryState state, int sourceIndex, EquipmentSlotId targetSlotId)
        {
            var hasMoved = false;
            var sourceSlot = GetInventorySlot(state, sourceIndex);
            var targetSlot = GetEquipmentSlot(state, targetSlotId);

            if (sourceSlot != null && targetSlot != null && CanEquipToSlot(sourceSlot.Item, targetSlot))
            {
                MoveInventoryItemToEquipment(sourceSlot, targetSlot);
                hasMoved = true;
            }

            return hasMoved;
        }

        public static bool TryMoveEquipmentToInventory(InventoryState state, EquipmentSlotId sourceSlotId, int targetIndex)
        {
            var hasMoved = false;
            var sourceSlot = GetEquipmentSlot(state, sourceSlotId);
            var targetSlot = GetInventorySlot(state, targetIndex);

            if (sourceSlot != null && targetSlot != null)
            {
                var sourceItem = sourceSlot.Item;
                var targetItem = targetSlot.Item;

                if (sourceItem != null && CanEquipToSlot(targetItem, sourceSlot))
                {
                    sourceSlot.SetItem(targetItem);
                    targetSlot.SetItem(sourceItem);
                    hasMoved = true;
                }
            }

            return hasMoved;
        }

        public static bool TryMoveEquipmentSlot(InventoryState state, EquipmentSlotId sourceSlotId, EquipmentSlotId targetSlotId)
        {
            var hasMoved = false;
            var sourceSlot = GetEquipmentSlot(state, sourceSlotId);
            var targetSlot = GetEquipmentSlot(state, targetSlotId);

            if (sourceSlot != null && targetSlot != null && sourceSlot != targetSlot && CanEquipToSlot(sourceSlot.Item, targetSlot) && CanEquipToSlot(targetSlot.Item, sourceSlot))
            {
                SwapItems(sourceSlot, targetSlot);
                hasMoved = true;
            }

            return hasMoved;
        }

        public static InventorySlotData GetInventorySlot(InventoryState state, int index)
        {
            InventorySlotData slot = null;

            if (index >= 0 && index < state.InventorySlots.Count)
            {
                slot = state.InventorySlots[index];
            }

            return slot;
        }

        public static EquipmentSlotData GetEquipmentSlot(InventoryState state, EquipmentSlotId slotId)
        {
            EquipmentSlotData slot = null;

            for (var i = 0; i < state.EquipmentSlots.Count; i++)
            {
                if (state.EquipmentSlots[i].SlotId == slotId)
                {
                    slot = state.EquipmentSlots[i];
                }
            }

            return slot;
        }

        public static InventoryStatSnapshot CalculateEquipmentStats(InventoryState state)
        {
            var snapshot = new InventoryStatSnapshot();

            for (var i = 0; i < state.EquipmentSlots.Count; i++)
            {
                var item = state.EquipmentSlots[i].Item;

                if (item != null)
                {
                    AppendItemStats(item.Preset, ref snapshot);
                }
            }

            return snapshot;
        }

        private static bool TryUseFromInventory(InventoryState state, int inventorySlotIndex, ref Health health)
        {
            var hasUsed = false;
            var inventorySlot = GetInventorySlot(state, inventorySlotIndex);

            if (inventorySlot != null && inventorySlot.Item != null)
            {
                var consumable = inventorySlot.Item.Preset as IConsumable;

                if (consumable != null && consumable.CanUse(health))
                {
                    consumable.Use(ref health);
                    ReduceItemCount(inventorySlot);
                    hasUsed = true;
                }
            }

            return hasUsed;
        }

        private static bool TryEquipNonTwoHandedItem(InventoryState state, InventorySlotData inventorySlot, IEquipmentItem equipmentItem)
        {
            var hasEquipped = false;

            if (equipmentItem.SlotType == EquipmentSlotType.OffHand)
            {
                hasEquipped = TryEquipOffHandItem(state, inventorySlot);
            }

            if (!hasEquipped && equipmentItem.SlotType != EquipmentSlotType.OffHand)
            {
                var equipmentSlot = GetAvailableEquipmentSlot(state, equipmentItem.SlotType);

                if (equipmentSlot != null)
                {
                    hasEquipped = MoveInventoryItemToEquipment(inventorySlot, equipmentSlot);
                }
            }

            return hasEquipped;
        }

        private static bool MoveInventoryItemToEquipment(InventorySlotData inventorySlot, EquipmentSlotData equipmentSlot)
        {
            var hasMoved = false;
            var inventoryItem = inventorySlot.Item;
            var equipmentItem = equipmentSlot.Item;

            if (inventoryItem != null)
            {
                equipmentSlot.SetItem(inventoryItem);
                inventorySlot.Clear();

                if (equipmentItem != null)
                {
                    inventorySlot.SetItem(equipmentItem);
                }

                hasMoved = true;
            }

            return hasMoved;
        }

        private static bool TryEquipTwoHandedItem(InventoryState state, InventorySlotData inventorySlot)
        {
            var hasEquipped = false;
            var mainHandSlot = GetEquipmentSlot(state, EquipmentSlotId.MainHand);
            var offHandSlot = GetEquipmentSlot(state, EquipmentSlotId.OffHand);

            if (CanEquipTwoHandedItem(state, inventorySlot, mainHandSlot, offHandSlot))
            {
                var twoHandedItem = inventorySlot.Item;
                var mainHandItem = mainHandSlot.Item;
                var offHandItem = offHandSlot.Item;
                mainHandSlot.SetItem(twoHandedItem);
                offHandSlot.Clear();
                inventorySlot.Clear();
                PlaceDisplacedHandItems(state, inventorySlot, mainHandItem, offHandItem);
                hasEquipped = true;
            }

            return hasEquipped;
        }

        private static bool TryEquipOffHandItem(InventoryState state, InventorySlotData inventorySlot)
        {
            var hasEquipped = false;
            var mainHandSlot = GetEquipmentSlot(state, EquipmentSlotId.MainHand);
            var offHandSlot = GetEquipmentSlot(state, EquipmentSlotId.OffHand);

            if (mainHandSlot != null && IsTwoHandedItem(mainHandSlot.Item))
            {
                var offHandItem = inventorySlot.Item;
                var twoHandedItem = mainHandSlot.Item;
                offHandSlot.SetItem(offHandItem);
                mainHandSlot.Clear();
                inventorySlot.SetItem(twoHandedItem);
                hasEquipped = true;
            }

            if (!hasEquipped && offHandSlot != null)
            {
                hasEquipped = MoveInventoryItemToEquipment(inventorySlot, offHandSlot);
            }

            return hasEquipped;
        }

        private static bool CanEquipTwoHandedItem(InventoryState state, InventorySlotData inventorySlot, EquipmentSlotData mainHandSlot, EquipmentSlotData offHandSlot)
        {
            var canEquip = false;

            if (inventorySlot != null && inventorySlot.Item != null && mainHandSlot != null && offHandSlot != null)
            {
                var displacedItemsCount = GetDisplacedItemsCount(mainHandSlot.Item, offHandSlot.Item);
                var freeSlotsCount = GetFreeInventorySlotsCount(state);
                canEquip = displacedItemsCount <= freeSlotsCount + 1;
            }

            return canEquip;
        }

        private static void PlaceDisplacedHandItems(InventoryState state, InventorySlotData sourceSlot, InventoryItemInstance mainHandItem, InventoryItemInstance offHandItem)
        {
            var hasUsedSourceSlot = false;

            if (mainHandItem != null)
            {
                sourceSlot.SetItem(mainHandItem);
                hasUsedSourceSlot = true;
            }

            if (offHandItem != null && !hasUsedSourceSlot)
            {
                sourceSlot.SetItem(offHandItem);
                hasUsedSourceSlot = true;
            }

            if (offHandItem != null && hasUsedSourceSlot && sourceSlot.Item != offHandItem)
            {
                var freeSlot = GetFreeInventorySlot(state);
                freeSlot.SetItem(offHandItem);
            }
        }

        private static int GetDisplacedItemsCount(InventoryItemInstance mainHandItem, InventoryItemInstance offHandItem)
        {
            var count = 0;

            if (mainHandItem != null)
            {
                count++;
            }

            if (offHandItem != null)
            {
                count++;
            }

            return count;
        }

        private static bool IsTwoHandedItem(InventoryItemInstance item)
        {
            var isTwoHanded = false;

            if (item != null)
            {
                var equipmentItem = item.Preset as IEquipmentItem;

                if (equipmentItem != null)
                {
                    isTwoHanded = equipmentItem.SlotType == EquipmentSlotType.TwoHanded;
                }
            }

            return isTwoHanded;
        }

        private static EquipmentSlotData GetAvailableEquipmentSlot(InventoryState state, EquipmentSlotType slotType)
        {
            EquipmentSlotData slot = null;

            for (var i = 0; i < state.EquipmentSlots.Count; i++)
            {
                var isSameType = state.EquipmentSlots[i].SlotType == slotType;
                var isFree = !state.EquipmentSlots[i].HasItem;

                if (isSameType && isFree && slot == null)
                {
                    slot = state.EquipmentSlots[i];
                }
            }

            if (slot == null)
            {
                slot = GetFirstEquipmentSlot(state, slotType);
            }

            return slot;
        }

        private static EquipmentSlotData GetFirstEquipmentSlot(InventoryState state, EquipmentSlotType slotType)
        {
            EquipmentSlotData slot = null;

            for (var i = 0; i < state.EquipmentSlots.Count; i++)
            {
                if (state.EquipmentSlots[i].SlotType == slotType && slot == null)
                {
                    slot = state.EquipmentSlots[i];
                }
            }

            return slot;
        }

        private static InventorySlotData GetFreeInventorySlot(InventoryState state)
        {
            InventorySlotData slot = null;

            for (var i = 0; i < state.InventorySlots.Count; i++)
            {
                if (!state.InventorySlots[i].HasItem && slot == null)
                {
                    slot = state.InventorySlots[i];
                }
            }

            return slot;
        }

        private static int GetFreeInventorySlotsCount(InventoryState state)
        {
            var count = 0;

            for (var i = 0; i < state.InventorySlots.Count; i++)
            {
                if (!state.InventorySlots[i].HasItem)
                {
                    count++;
                }
            }

            return count;
        }

        private static bool TryAddToExistingStack(InventoryState state, InventoryItemInstance item)
        {
            var hasAdded = false;

            for (var i = 0; i < state.InventorySlots.Count; i++)
            {
                var slotItem = state.InventorySlots[i].Item;

                if (CanStack(slotItem, item) && !hasAdded)
                {
                    var freeCount = slotItem.Preset.MaxStack - slotItem.Count;
                    var addedCount = item.Count <= freeCount ? item.Count : freeCount;
                    slotItem.SetCount(slotItem.Count + addedCount);
                    item.SetCount(item.Count - addedCount);
                    hasAdded = item.Count <= 0;
                }
            }

            return hasAdded;
        }

        private static bool CanStack(InventoryItemInstance left, InventoryItemInstance right)
        {
            var canStack = false;

            if (left != null && right != null)
            {
                var isSamePreset = left.Preset == right.Preset;
                var hasSpace = left.Count < left.Preset.MaxStack;
                canStack = isSamePreset && hasSpace;
            }

            return canStack;
        }

        private static bool HasInstance(InventoryState state, long instanceId)
        {
            var hasInstance = false;

            for (var i = 0; i < state.InventorySlots.Count; i++)
            {
                if (state.InventorySlots[i].Item != null && state.InventorySlots[i].Item.InstanceId == instanceId)
                {
                    hasInstance = true;
                }
            }

            for (var i = 0; i < state.EquipmentSlots.Count; i++)
            {
                if (state.EquipmentSlots[i].Item != null && state.EquipmentSlots[i].Item.InstanceId == instanceId)
                {
                    hasInstance = true;
                }
            }

            return hasInstance;
        }

        private static void ReduceItemCount(InventorySlotData slot)
        {
            var count = slot.Item.Count - 1;
            slot.Item.SetCount(count);

            if (slot.Item.Count <= 0)
            {
                slot.Clear();
            }
        }

        private static bool CanEquipToSlot(InventoryItemInstance item, EquipmentSlotData slot)
        {
            var canEquip = item == null;

            if (item != null)
            {
                var equipmentItem = item.Preset as IEquipmentItem;

                if (equipmentItem != null)
                {
                    canEquip = slot.SlotType == equipmentItem.SlotType;
                }

                if (equipmentItem != null && equipmentItem.SlotType == EquipmentSlotType.TwoHanded)
                {
                    canEquip = slot.SlotId == EquipmentSlotId.MainHand;
                }
            }

            return canEquip;
        }

        private static void SwapItems(InventorySlotData left, InventorySlotData right)
        {
            var leftItem = left.Item;
            left.SetItem(right.Item);
            right.SetItem(leftItem);
        }

        private static void SwapItems(EquipmentSlotData left, EquipmentSlotData right)
        {
            var leftItem = left.Item;
            left.SetItem(right.Item);
            right.SetItem(leftItem);
        }

        private static void AppendItemStats(ItemPreset preset, ref InventoryStatSnapshot snapshot)
        {
            var armorItem = preset as IArmor;

            if (armorItem != null)
            {
                snapshot._armorBonus += Mathf.Max(0f, armorItem.Armor);
            }

            for (var i = 0; i < preset.StatModifiers.Count; i++)
            {
                if (preset.StatModifiers[i] != null)
                {
                    preset.StatModifiers[i].Append(ref snapshot);
                }
            }
        }

        private static void AddEquipmentSlot(InventoryState state, EquipmentSlotId slotId, EquipmentSlotType slotType)
        {
            state.EquipmentSlots.Add(new EquipmentSlotData(slotId, slotType));
        }
    }
}


