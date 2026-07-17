using UnityEngine;
using UnityEngine.InputSystem;

namespace ECSMiniRPG.InputSystem
{
    public sealed class NewInput
    {
        private static readonly string _playerMapName = "Player";
        private static readonly string _guiMapName = "GUI";
        private static readonly string _moveActionName = "Move";
        private static readonly string _interactActionName = "Interact";
        private static readonly string _pickUpActionName = "PickUp";
        private static readonly string _pauseActionName = "Pause";
        private static readonly string _inventoryActionName = "Inventory";

        private readonly InputActionAsset _asset;

        public NewInput(InputActionAsset asset)
        {
            _asset = asset;
            Player = new PlayerActions(asset.FindActionMap(_playerMapName, true));
            GUI = new GUIActions(asset.FindActionMap(_guiMapName, true));
        }

        public PlayerActions Player { get; }
        public GUIActions GUI { get; }

        public void Enable()
        {
            _asset.Enable();
        }

        public void Disable()
        {
            _asset.Disable();
        }

        public readonly struct PlayerActions
        {
            private readonly InputActionMap _map;

            public PlayerActions(InputActionMap map)
            {
                _map = map;
                Move = _map.FindAction(_moveActionName, true);
                Interact = _map.FindAction(_interactActionName, true);
                PickUp = _map.FindAction(_pickUpActionName, true);
            }

            public InputAction Move { get; }
            public InputAction Interact { get; }
            public InputAction PickUp { get; }

            public void Enable()
            {
                _map.Enable();
            }

            public void Disable()
            {
                _map.Disable();
            }

            public Vector2 ReadMove()
            {
                return Move.ReadValue<Vector2>();
            }

            public bool WasInteractPressedThisFrame()
            {
                return Interact.WasPressedThisFrame();
            }

            public bool IsPickUpPressed()
            {
                return PickUp.IsPressed();
            }
        }

        public readonly struct GUIActions
        {
            private readonly InputActionMap _map;

            public GUIActions(InputActionMap map)
            {
                _map = map;
                Pause = _map.FindAction(_pauseActionName, true);
                Inventory = _map.FindAction(_inventoryActionName, true);
            }

            public InputAction Pause { get; }
            public InputAction Inventory { get; }

            public bool WasPausePressedThisFrame()
            {
                return Pause.WasPressedThisFrame();
            }

            public bool WasInventoryPressedThisFrame()
            {
                return Inventory.WasPressedThisFrame();
            }
        }
    }
}


