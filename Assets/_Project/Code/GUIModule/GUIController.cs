using System.Collections.Generic;
using ECSMiniRPG.GUIModule.Elements;
using ECSMiniRPG.InputSystem;
using ECSMiniRPG.InventoryModule;
using ECSMiniRPG.InventoryModule.Components;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ECSMiniRPG.GUIModule
{
    public sealed class GUIController : IResource
    {
        private static readonly string _mainMenuSceneName = "Menu";

        private readonly GUIView _view;
        private readonly NewInputProvider _inputProvider;
        private readonly List<GUIPanel> _panels = new List<GUIPanel>();
        private PauseMenuView _pauseMenuView;
        private InventoryView _inventoryView;
        private PauseMenuElement _pauseMenuElement;
        private InventoryPanelElement _inventoryPanelElement;
        private bool _isInitialized;

        public GUIController(GUIView view, NewInputProvider inputProvider)
        {
            _view = view;
            _inputProvider = inputProvider;
        }

        public void Initialize()
        {
            _pauseMenuView = _view.PauseMenuView;
            _inventoryView = _view.InventoryView;
            _pauseMenuElement = new PauseMenuElement(_pauseMenuView.Panel);
            _inventoryPanelElement = new InventoryPanelElement(_inventoryView.Panel);
            _panels.Add(_pauseMenuElement);
            _panels.Add(_inventoryPanelElement);
            BindPauseMenu();
            BindInventory();
            UpdateInventoryView();
            UpdatePlayerInputBlock();
            _isInitialized = true;
        }

        public void Update()
        {
            if (_inputProvider.WasPausePressedThisFrame())
            {
                _pauseMenuElement.Toggle();
                UpdatePlayerInputBlock();
            }

            if (_inputProvider.WasInventoryPressedThisFrame())
            {
                _inventoryPanelElement.Toggle();
                UpdateInventoryView();
                UpdatePlayerInputBlock();
            }

            if (_inventoryPanelElement.IsOpen)
            {
                UpdateInventoryView();
            }
        }

        public void Destroy()
        {
            if (_isInitialized)
            {
                UnbindPauseMenu();
                UnbindInventory();
                _inputProvider.SetPlayerInputBlocked(false);
                _isInitialized = false;
            }
        }

        private void BindPauseMenu()
        {
            _pauseMenuView.ResumeButton.clicked += ClosePauseMenu;
            _pauseMenuView.MainMenuButton.clicked += LoadMainMenu;
            _pauseMenuView.ExitButton.clicked += StopApplication;
        }

        private void UnbindPauseMenu()
        {
            _pauseMenuView.ResumeButton.clicked -= ClosePauseMenu;
            _pauseMenuView.MainMenuButton.clicked -= LoadMainMenu;
            _pauseMenuView.ExitButton.clicked -= StopApplication;
        }

        private void BindInventory()
        {
            _inventoryView.InventorySlotDoubleClicked += OnInventorySlotDoubleClicked;
            _inventoryView.EquipmentSlotDoubleClicked += OnEquipmentSlotDoubleClicked;
            _inventoryView.ItemDropped += OnInventoryItemDropped;
            _inventoryView.CloseClicked += CloseInventory;
            _inventoryView.Bind();
        }

        private void UnbindInventory()
        {
            _inventoryView.InventorySlotDoubleClicked -= OnInventorySlotDoubleClicked;
            _inventoryView.EquipmentSlotDoubleClicked -= OnEquipmentSlotDoubleClicked;
            _inventoryView.ItemDropped -= OnInventoryItemDropped;
            _inventoryView.CloseClicked -= CloseInventory;
            _inventoryView.Unbind();
        }

        private void ClosePauseMenu()
        {
            _pauseMenuElement.Close();
            UpdatePlayerInputBlock();
        }

        private void CloseInventory()
        {
            _inventoryPanelElement.Close();
            UpdatePlayerInputBlock();
        }

        private void OnInventorySlotDoubleClicked(int slotIndex)
        {
            EnqueueInventoryCommand(InventoryCommand.UseOrEquipInventorySlot(slotIndex));
        }

        private void OnEquipmentSlotDoubleClicked(EquipmentSlotId slotId)
        {
            EnqueueInventoryCommand(InventoryCommand.UnequipEquipmentSlot(slotId));
        }

        private void OnInventoryItemDropped(InventoryDragData sourceData, InventoryDragData targetData)
        {
            if (sourceData._sourceType == InventorySlotSourceType.Inventory && targetData._sourceType == InventorySlotSourceType.Inventory)
            {
                EnqueueInventoryCommand(InventoryCommand.MoveInventorySlot(sourceData._inventoryIndex, targetData._inventoryIndex));
            }

            if (sourceData._sourceType == InventorySlotSourceType.Inventory && targetData._sourceType == InventorySlotSourceType.Equipment)
            {
                EnqueueInventoryCommand(InventoryCommand.MoveInventoryToEquipment(sourceData._inventoryIndex, targetData._equipmentSlotId));
            }

            if (sourceData._sourceType == InventorySlotSourceType.Equipment && targetData._sourceType == InventorySlotSourceType.Inventory)
            {
                EnqueueInventoryCommand(InventoryCommand.MoveEquipmentToInventory(sourceData._equipmentSlotId, targetData._inventoryIndex));
            }

            if (sourceData._sourceType == InventorySlotSourceType.Equipment && targetData._sourceType == InventorySlotSourceType.Equipment)
            {
                EnqueueInventoryCommand(InventoryCommand.MoveEquipmentSlot(sourceData._equipmentSlotId, targetData._equipmentSlotId));
            }
        }

        private void EnqueueInventoryCommand(InventoryCommand command)
        {
            ref var commandQueue = ref GameWorld.GetResource<InventoryCommandQueue>();
            commandQueue.Enqueue(command);
        }

        private void UpdateInventoryView()
        {
            foreach (var entity in GameWorld.Query<All<PlayerTag, InventoryComponent>>().Entities())
            {
                ref readonly var inventory = ref entity.Read<InventoryComponent>();

                if (inventory._isInitialized)
                {
                    _inventoryView.ApplyState(inventory._state);
                }
            }
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        private void StopApplication()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void UpdatePlayerInputBlock()
        {
            var isPlayerInputBlocked = false;

            for (var i = 0; i < _panels.Count; i++)
            {
                if (_panels[i].IsOpen && _panels[i].IsPlayerInputBlocked)
                {
                    isPlayerInputBlocked = true;
                }
            }

            _inputProvider.SetPlayerInputBlocked(isPlayerInputBlocked);
        }
    }
}
