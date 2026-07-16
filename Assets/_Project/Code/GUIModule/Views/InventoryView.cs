using System;
using System.Collections.Generic;
using ECSMiniRPG.InventoryModule;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECSMiniRPG.GUIModule
{
    public sealed class InventoryView : MonoBehaviour
    {
        private static readonly string _rootName = "inventory-root";
        private static readonly string _gridName = "inventory-grid";
        private static readonly string _equipmentName = "equipment-slots";
        private static readonly string _closeButtonName = "inventory-close-button";
        private static readonly string _slotClass = "inventory-slot";
        private static readonly string _equipmentSlotClass = "equipment-slot";
        private static readonly string _itemTitleClass = "inventory-item-title";
        private static readonly string _itemCountClass = "inventory-item-count";
        private static readonly string _itemIconClass = "inventory-item-icon";
        private static readonly float _slotSize = 64f;
        private static readonly float _slotMargin = 2f;

        [SerializeField] private UIDocument _document;

        private readonly Dictionary<EquipmentSlotId, VisualElement> _equipmentSlotElements = new Dictionary<EquipmentSlotId, VisualElement>();
        private VisualElement _root;
        private VisualElement _grid;
        private VisualElement _equipment;
        private Button _closeButton;
        private InventoryDragData _dragData;

        public event Action<int> InventorySlotDoubleClicked;
        public event Action<EquipmentSlotId> EquipmentSlotDoubleClicked;
        public event Action<InventoryDragData, InventoryDragData> ItemDropped;
        public event Action CloseClicked;

        public bool IsInitialized { get; private set; }
        public VisualElement Panel => _root;

        public void Initialize()
        {
            Initialize(_document);
        }

        public void Initialize(UIDocument document)
        {
            _document = document;

            if (!IsInitialized && _document != null)
            {
                CacheElements();
                IsInitialized = _root != null && _grid != null && _equipment != null && _closeButton != null;
            }

            if (_document == null)
            {
                Debug.LogError("UIDocument is not assigned in InventoryView.");
            }

            if (_document != null && !IsInitialized)
            {
                Debug.LogError("InventoryView cannot find required UI elements.");
            }
        }

        public void Bind()
        {
            if (_closeButton != null)
            {
                _closeButton.clicked += OnCloseClicked;
            }
        }

        public void Unbind()
        {
            if (_closeButton != null)
            {
                _closeButton.clicked -= OnCloseClicked;
            }
        }

        public void ApplyState(InventoryState inventoryState)
        {
            if (IsInitialized)
            {
                RebuildEquipment(inventoryState);
                RebuildInventoryGrid(inventoryState);
            }
        }

        private void CacheElements()
        {
            _root = _document.rootVisualElement.Q<VisualElement>(_rootName);
            _grid = _document.rootVisualElement.Q<VisualElement>(_gridName);
            _equipment = _document.rootVisualElement.Q<VisualElement>(_equipmentName);
            _closeButton = _document.rootVisualElement.Q<Button>(_closeButtonName);
            CacheEquipmentSlots();
        }

        private void RebuildInventoryGrid(InventoryState inventoryState)
        {
            _grid.Clear();

            for (var i = 0; i < inventoryState.InventorySlots.Count; i++)
            {
                _grid.Add(CreateInventorySlot(inventoryState.InventorySlots[i]));
            }
        }

        private void RebuildEquipment(InventoryState inventoryState)
        {
            ClearEquipmentSlots();

            for (var i = 0; i < inventoryState.EquipmentSlots.Count; i++)
            {
                ApplyEquipmentSlot(inventoryState.EquipmentSlots[i]);
            }
        }

        private VisualElement CreateInventorySlot(InventorySlotData slotData)
        {
            var slot = CreateSlotElement(slotData.Item, true, true);
            slot.AddToClassList(_slotClass);
            slot.RegisterCallback<MouseDownEvent>(evt => OnInventorySlotMouseDown(evt, slotData.Index));
            slot.RegisterCallback<PointerDownEvent>(evt => OnInventorySlotPointerDown(slotData.Index));
            slot.RegisterCallback<PointerUpEvent>(evt => OnInventorySlotPointerUp(slotData.Index));
            return slot;
        }

        private VisualElement CreateSlotElement(InventoryItemInstance item, bool shouldShowTitle, bool shouldUseMargin)
        {
            var slot = new VisualElement();
            slot.style.width = _slotSize;
            slot.style.height = _slotSize;

            if (shouldUseMargin)
            {
                slot.style.marginBottom = _slotMargin;
                slot.style.marginLeft = _slotMargin;
                slot.style.marginRight = _slotMargin;
                slot.style.marginTop = _slotMargin;
            }

            if (item != null)
            {
                AddItemIcon(slot, item);
                AddItemText(slot, item, shouldShowTitle);
            }

            return slot;
        }

        private void AddItemIcon(VisualElement slot, InventoryItemInstance item)
        {
            if (item.Preset.Icon != null)
            {
                var icon = new VisualElement();
                icon.AddToClassList(_itemIconClass);
                icon.style.backgroundImage = new StyleBackground(item.Preset.Icon.texture);
                slot.Add(icon);
            }
        }

        private void AddItemText(VisualElement slot, InventoryItemInstance item, bool shouldShowTitle)
        {
            if (shouldShowTitle)
            {
                var title = new Label(item.Preset.Title);
                title.AddToClassList(_itemTitleClass);
                slot.Add(title);
            }

            if (item.Count > 1)
            {
                var count = new Label(item.Count.ToString());
                count.AddToClassList(_itemCountClass);
                slot.Add(count);
            }
        }

        private void CacheEquipmentSlots()
        {
            _equipmentSlotElements.Clear();

            AddEquipmentSlotElement(EquipmentSlotId.Head);
            AddEquipmentSlotElement(EquipmentSlotId.Chest);
            AddEquipmentSlotElement(EquipmentSlotId.Shoulders);
            AddEquipmentSlotElement(EquipmentSlotId.Gloves);
            AddEquipmentSlotElement(EquipmentSlotId.Pants);
            AddEquipmentSlotElement(EquipmentSlotId.Boots);
            AddEquipmentSlotElement(EquipmentSlotId.RingLeft);
            AddEquipmentSlotElement(EquipmentSlotId.RingRight);
            AddEquipmentSlotElement(EquipmentSlotId.Amulet);
            AddEquipmentSlotElement(EquipmentSlotId.MainHand);
            AddEquipmentSlotElement(EquipmentSlotId.OffHand);
            AddEquipmentSlotElement(EquipmentSlotId.QuickConsumable1);
            AddEquipmentSlotElement(EquipmentSlotId.QuickConsumable2);
        }

        private void AddEquipmentSlotElement(EquipmentSlotId slotId)
        {
            var slotName = GetEquipmentSlotName(slotId);
            var slot = _equipment.Q<VisualElement>(slotName);

            if (slot != null)
            {
                slot.tooltip = slotId.ToString();
                slot.AddToClassList(_equipmentSlotClass);
                slot.RegisterCallback<MouseDownEvent>(evt => OnEquipmentSlotMouseDown(evt, slotId));
                slot.RegisterCallback<PointerDownEvent>(evt => OnEquipmentSlotPointerDown(slotId));
                slot.RegisterCallback<PointerUpEvent>(evt => OnEquipmentSlotPointerUp(slotId));
                _equipmentSlotElements.Add(slotId, slot);
            }
        }

        private void ClearEquipmentSlots()
        {
            foreach (var slot in _equipmentSlotElements)
            {
                slot.Value.Clear();
            }
        }

        private void ApplyEquipmentSlot(EquipmentSlotData slotData)
        {
            if (_equipmentSlotElements.ContainsKey(slotData.SlotId) && slotData.Item != null)
            {
                var slot = _equipmentSlotElements[slotData.SlotId];
                AddItemIcon(slot, slotData.Item);
                AddItemText(slot, slotData.Item, false);
            }
        }

        private void OnInventorySlotMouseDown(MouseDownEvent evt, int slotIndex)
        {
            if (evt.clickCount >= 2)
            {
                if (InventorySlotDoubleClicked != null)
                {
                    InventorySlotDoubleClicked.Invoke(slotIndex);
                }
            }
        }

        private void OnEquipmentSlotMouseDown(MouseDownEvent evt, EquipmentSlotId slotId)
        {
            if (evt.clickCount >= 2)
            {
                if (EquipmentSlotDoubleClicked != null)
                {
                    EquipmentSlotDoubleClicked.Invoke(slotId);
                }
            }
        }

        private void OnInventorySlotPointerDown(int slotIndex)
        {
            _dragData = InventoryDragData.Inventory(slotIndex);
        }

        private void OnInventorySlotPointerUp(int slotIndex)
        {
            DropTo(InventoryDragData.Inventory(slotIndex));
        }

        private void OnEquipmentSlotPointerDown(EquipmentSlotId slotId)
        {
            _dragData = InventoryDragData.Equipment(slotId);
        }

        private void OnEquipmentSlotPointerUp(EquipmentSlotId slotId)
        {
            DropTo(InventoryDragData.Equipment(slotId));
        }

        private void DropTo(InventoryDragData targetData)
        {
            if (_dragData._sourceType != InventorySlotSourceType.None)
            {
                if (ItemDropped != null)
                {
                    ItemDropped.Invoke(_dragData, targetData);
                }
                _dragData = default;
            }
        }

        private void OnCloseClicked()
        {
            if (CloseClicked != null)
            {
                CloseClicked.Invoke();
            }
        }

        private string GetEquipmentSlotName(EquipmentSlotId slotId)
        {
            var slotName = string.Empty;

            if (slotId == EquipmentSlotId.Head)
            {
                slotName = "slot-equip-head";
            }

            if (slotId == EquipmentSlotId.Chest)
            {
                slotName = "slot-equip-chest";
            }

            if (slotId == EquipmentSlotId.Shoulders)
            {
                slotName = "slot-equip-shoulders";
            }

            if (slotId == EquipmentSlotId.Gloves)
            {
                slotName = "slot-equip-gloves";
            }

            if (slotId == EquipmentSlotId.Pants)
            {
                slotName = "slot-equip-pants";
            }

            if (slotId == EquipmentSlotId.Boots)
            {
                slotName = "slot-equip-boots";
            }

            if (slotId == EquipmentSlotId.RingLeft)
            {
                slotName = "slot-equip-ring-left";
            }

            if (slotId == EquipmentSlotId.RingRight)
            {
                slotName = "slot-equip-ring-right";
            }

            if (slotId == EquipmentSlotId.Amulet)
            {
                slotName = "slot-equip-amulet";
            }

            if (slotId == EquipmentSlotId.MainHand)
            {
                slotName = "slot-equip-main-hand";
            }

            if (slotId == EquipmentSlotId.OffHand)
            {
                slotName = "slot-equip-off-hand";
            }

            if (slotId == EquipmentSlotId.QuickConsumable1)
            {
                slotName = "slot-equip-quick-consumable-1";
            }

            if (slotId == EquipmentSlotId.QuickConsumable2)
            {
                slotName = "slot-equip-quick-consumable-2";
            }

            return slotName;
        }
    }
}



