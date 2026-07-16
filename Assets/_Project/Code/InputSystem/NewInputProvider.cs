using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECSMiniRPG.InputSystem
{
    public sealed class NewInputProvider : IResource
    {
        private readonly NewInput _newInput;
        private bool _isPlayerInputBlocked;

        public NewInputProvider(InputActionAsset inputActionAsset)
        {
            _newInput = new NewInput(inputActionAsset);
        }

        public Vector2 MoveDirection => _isPlayerInputBlocked ? Vector2.zero : _newInput.Player.ReadMove();
        public bool IsPlayerInputBlocked => _isPlayerInputBlocked;

        public bool WasPausePressedThisFrame()
        {
            return _newInput.GUI.WasPausePressedThisFrame();
        }

        public bool WasInventoryPressedThisFrame()
        {
            return _newInput.GUI.WasInventoryPressedThisFrame();
        }

        public void SetPlayerInputBlocked(bool isBlocked)
        {
            if (_isPlayerInputBlocked != isBlocked)
            {
                _isPlayerInputBlocked = isBlocked;
                ApplyPlayerInputState();
            }
        }

        public void Enable()
        {
            _newInput.Enable();
            ApplyPlayerInputState();
        }

        public void Disable()
        {
            _newInput.Disable();
        }

        private void ApplyPlayerInputState()
        {
            if (_isPlayerInputBlocked)
            {
                _newInput.Player.Disable();
            }

            if (!_isPlayerInputBlocked)
            {
                _newInput.Player.Enable();
            }
        }
    }
}

