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
        private static readonly string _panelName = "inventory-panel";
        private static readonly string _closeButtonName = "inventory-close-button";
        private static readonly string _slotClass = "inventory-slot";
        private static readonly string _equipmentSlotClass = "equipment-slot";
        private static readonly string _itemTitleClass = "inventory-item-title";
        private static readonly string _itemCountClass = "inventory-item-count";
        private static readonly string _itemIconClass = "inventory-item-icon";
        private static readonly string _dropDialogClass = "inventory-drop-dialog";
        private static readonly string _dropDialogWindowClass = "inventory-drop-dialog-window";
        private static readonly string _dropDialogLabelClass = "inventory-drop-dialog-label";
        private static readonly string _dropDialogButtonsClass = "inventory-drop-dialog-buttons";
        private static readonly string _dropDialogSliderClass = "inventory-drop-dialog-slider";
        private static readonly string _buttonBaseClass = "button-base";
        private static readonly string _buttonDangerClass = "button-danger";
        private static readonly string _buttonSecondaryClass = "button-secondary";
        private static readonly string _buttonSizeSmallClass = "button-size-s";
        private static readonly string _dropButtonText = "Drop";
        private static readonly string _cancelButtonText = "Cancel";
        private static readonly float _slotSize = 64f;
        private static readonly float _slotMargin = 2f;
        private static readonly int _emptyDropCount = 0;
        private static readonly int _minDropCount = 1;

        [SerializeField] private UIDocument _document;

        private readonly Dictionary<EquipmentSlotId, VisualElement> _equipmentSlotElements = new Dictionary<EquipmentSlotId, VisualElement>();
        private VisualElement _documentRoot;
        private VisualElement _root;
        private VisualElement _panel;
        private VisualElement _grid;
        private VisualElement _equipment;
        private Button _closeButton;
        private VisualElement _dropDialog;
        private Label _dropCountLabel;
        private SliderInt _dropSlider;
        private Button _dropButton;
        private Button _cancelDropButton;
        private InventoryDragData _dragData;
        private InventoryDragData _hoveredData;
        private InventoryDragData _dropDialogSourceData;
        private int _dropDialogMaxCount;
        private int _dragPointerId;

        public event Action<int> InventorySlotDoubleClicked;
        public event Action<EquipmentSlotId> EquipmentSlotDoubleClicked;
        public event Action<InventoryDragData, InventoryDragData> ItemDropped;
        public event Action<InventoryDragData> ItemDroppedOutside;
        public event Action<InventoryDragData, int> ItemDropAmountRequested;
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
                CreateDropDialog();
                IsInitialized = _root != null && _panel != null && _grid != null && _equipment != null && _closeButton != null;
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

            if (_root != null)
            {
                _documentRoot.RegisterCallback<PointerUpEvent>(OnRootPointerUp);
                _root.RegisterCallback<KeyDownEvent>(OnRootKeyDown);
                _root.focusable = true;
            }
        }

        public void Unbind()
        {
            if (_closeButton != null)
            {
                _closeButton.clicked -= OnCloseClicked;
            }

            if (_root != null)
            {
                _documentRoot.UnregisterCallback<PointerUpEvent>(OnRootPointerUp);
                _root.UnregisterCallback<KeyDownEvent>(OnRootKeyDown);
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

        public void OpenDropDialog(InventoryDragData sourceData, int maxCount)
        {
            if (_dropDialog != null && maxCount >= _minDropCount)
            {
                _dropDialogSourceData = sourceData;
                _dropDialogMaxCount = maxCount;
                _dropSlider.lowValue = _minDropCount;
                _dropSlider.highValue = maxCount;
                _dropSlider.value = maxCount;
                UpdateDropCountLabel();
                _dropDialog.style.display = DisplayStyle.Flex;
            }
        }

        private void CacheElements()
        {
            _documentRoot = _document.rootVisualElement;
            _root = _documentRoot.Q<VisualElement>(_rootName);
            _panel = _documentRoot.Q<VisualElement>(_panelName);
            _grid = _documentRoot.Q<VisualElement>(_gridName);
            _equipment = _documentRoot.Q<VisualElement>(_equipmentName);
            _closeButton = _documentRoot.Q<Button>(_closeButtonName);
            CacheEquipmentSlots();
        }

        private void CreateDropDialog()
        {
            if (_documentRoot != null)
            {
                _dropDialog = new VisualElement();
                _dropDialog.AddToClassList(_dropDialogClass);
                _dropDialogSourceData = default;
                _dropDialog.style.display = DisplayStyle.None;

                var window = new VisualElement();
                window.AddToClassList(_dropDialogWindowClass);

                _dropCountLabel = new Label();
                _dropCountLabel.AddToClassList(_dropDialogLabelClass);
                window.Add(_dropCountLabel);

                _dropSlider = new SliderInt(_minDropCount, _minDropCount);
                _dropSlider.AddToClassList(_dropDialogSliderClass);
                _dropSlider.RegisterValueChangedCallback(evt => UpdateDropCountLabel());
                window.Add(_dropSlider);

                var buttons = new VisualElement();
                buttons.AddToClassList(_dropDialogButtonsClass);
                _dropButton = new Button(OnDropDialogDropClicked) { text = _dropButtonText };
                _dropButton.AddToClassList(_buttonBaseClass);
                _dropButton.AddToClassList(_buttonDangerClass);
                _dropButton.AddToClassList(_buttonSizeSmallClass);
                _cancelDropButton = new Button(CloseDropDialog) { text = _cancelButtonText };
                _cancelDropButton.AddToClassList(_buttonBaseClass);
                _cancelDropButton.AddToClassList(_buttonSecondaryClass);
                _cancelDropButton.AddToClassList(_buttonSizeSmallClass);
                buttons.Add(_dropButton);
                buttons.Add(_cancelDropButton);
                window.Add(buttons);
                _dropDialog.Add(window);
                _documentRoot.Add(_dropDialog);
            }
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
            slot.RegisterCallback<PointerDownEvent>(evt => OnInventorySlotPointerDown(evt, slotData.Index));
            slot.RegisterCallback<PointerUpEvent>(evt => OnInventorySlotPointerUp(evt, slotData.Index));
            slot.RegisterCallback<PointerEnterEvent>(evt => SetHoveredData(InventoryDragData.Inventory(slotData.Index)));
            slot.RegisterCallback<PointerLeaveEvent>(evt => ClearHoveredData(InventoryDragData.Inventory(slotData.Index)));
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
                slot.RegisterCallback<PointerDownEvent>(evt => OnEquipmentSlotPointerDown(evt, slotId));
                slot.RegisterCallback<PointerUpEvent>(evt => OnEquipmentSlotPointerUp(evt, slotId));
                slot.RegisterCallback<PointerEnterEvent>(evt => SetHoveredData(InventoryDragData.Equipment(slotId)));
                slot.RegisterCallback<PointerLeaveEvent>(evt => ClearHoveredData(InventoryDragData.Equipment(slotId)));
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

        private void OnInventorySlotPointerDown(PointerDownEvent evt, int slotIndex)
        {
            _dragData = InventoryDragData.Inventory(slotIndex);
            CaptureDragPointer(evt.pointerId);
            FocusRoot();
        }

        private void OnInventorySlotPointerUp(PointerUpEvent evt, int slotIndex)
        {
            DropTo(InventoryDragData.Inventory(slotIndex));
            ReleaseDragPointer(evt.pointerId);
        }

        private void OnEquipmentSlotPointerDown(PointerDownEvent evt, EquipmentSlotId slotId)
        {
            _dragData = InventoryDragData.Equipment(slotId);
            CaptureDragPointer(evt.pointerId);
            FocusRoot();
        }

        private void OnEquipmentSlotPointerUp(PointerUpEvent evt, EquipmentSlotId slotId)
        {
            DropTo(InventoryDragData.Equipment(slotId));
            ReleaseDragPointer(evt.pointerId);
        }
        private void OnRootPointerUp(PointerUpEvent evt)
        {
            if (_dragData._sourceType != InventorySlotSourceType.None)
            {
                var isOutsidePanel = !_panel.worldBound.Contains(evt.position);

                if (isOutsidePanel)
                {
                    if (ItemDroppedOutside != null)
                    {
                        ItemDroppedOutside.Invoke(_dragData);
                    }

                    _dragData = default;
                }

                ReleaseDragPointer(evt.pointerId);
            }
        }
        private void OnRootKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Q && _hoveredData._sourceType != InventorySlotSourceType.None)
            {
                if (ItemDropAmountRequested != null)
                {
                    ItemDropAmountRequested.Invoke(_hoveredData, _emptyDropCount);
                }
            }
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

        private void SetHoveredData(InventoryDragData data)
        {
            _hoveredData = data;
            FocusRoot();
        }

        private void ClearHoveredData(InventoryDragData data)
        {
            if (IsSameData(_hoveredData, data))
            {
                _hoveredData = default;
            }
        }

        private bool IsSameData(InventoryDragData left, InventoryDragData right)
        {
            return left._sourceType == right._sourceType && left._inventoryIndex == right._inventoryIndex && left._equipmentSlotId == right._equipmentSlotId;
        }

        private void OnDropDialogDropClicked()
        {
            if (ItemDropAmountRequested != null)
            {
                ItemDropAmountRequested.Invoke(_dropDialogSourceData, _dropSlider.value);
            }

            CloseDropDialog();
        }

        private void CloseDropDialog()
        {
            if (_dropDialog != null)
            {
                _dropDialogSourceData = default;
                _dropDialog.style.display = DisplayStyle.None;
            }
        }

        private void UpdateDropCountLabel()
        {
            if (_dropCountLabel != null)
            {
                _dropCountLabel.text = $"{_dropSlider.value}/{_dropDialogMaxCount}";
            }
        }

        private void FocusRoot()
        {
            if (_root != null)
            {
                _root.Focus();
            }
        }

        private void CaptureDragPointer(int pointerId)
        {
            _dragPointerId = pointerId;
            _documentRoot.CapturePointer(pointerId);
        }

        private void ReleaseDragPointer(int pointerId)
        {
            if (_dragPointerId == pointerId && _documentRoot.HasPointerCapture(pointerId))
            {
                _documentRoot.ReleasePointer(pointerId);
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