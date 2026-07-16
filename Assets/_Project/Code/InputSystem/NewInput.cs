using UnityEngine;
using UnityEngine.InputSystem;

namespace ECSMiniRPG.InputSystem
{
    public sealed class NewInput
    {
        private static readonly string _playerMapName = "Player";
        private static readonly string _guiMapName = "GUI";
        private static readonly string _moveActionName = "Move";
        private static readonly string _pauseActionName = "Pause";

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
            }

            public InputAction Move { get; }

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
        }

        public readonly struct GUIActions
        {
            private readonly InputActionMap _map;

            public GUIActions(InputActionMap map)
            {
                _map = map;
                Pause = _map.FindAction(_pauseActionName, true);
            }

            public InputAction Pause { get; }

            public bool WasPausePressedThisFrame()
            {
                return Pause.WasPressedThisFrame();
            }
        }
    }
}
