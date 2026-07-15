using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECSMiniRPG.InputSystem
{
    public sealed class NewInput
    {
        private static readonly string _playerMapName = "Player";
        private static readonly string _moveActionName = "Move";

        private readonly InputActionAsset _asset;

        public NewInput(InputActionAsset asset)
        {
            _asset = asset;
            Player = new PlayerActions(asset.FindActionMap(_playerMapName, true));
        }

        public PlayerActions Player
        {
            get;
        }

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

            public InputAction Move
            {
                get;
            }

            public Vector2 ReadMove()
            {
                return Move.ReadValue<Vector2>();
            }
        }
    }
}
