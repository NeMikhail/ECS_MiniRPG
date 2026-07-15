using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECSMiniRPG.InputSystem
{
    public sealed class NewInputProvider : IResource
    {
        private readonly NewInput _newInput;

        public NewInputProvider(InputActionAsset inputActionAsset)
        {
            _newInput = new NewInput(inputActionAsset);
        }

        public Vector2 MoveDirection => _newInput.Player.ReadMove();

        public void Enable()
        {
            _newInput.Enable();
        }

        public void Disable()
        {
            _newInput.Disable();
        }
    }
}
