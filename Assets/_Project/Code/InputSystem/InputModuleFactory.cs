using ECSMiniRPG.Core;
using ECSMiniRPG.InputSystem.Systems;
using UnityEngine.InputSystem;

namespace ECSMiniRPG.InputSystem
{
    public sealed class InputModuleFactory : IEcsModuleFactory
    {
        private static readonly short _inputSystemOrder = 10;

        private readonly InputActionAsset _inputActionAsset;
        private NewInputProvider _newInputProvider;

        public InputModuleFactory(InputActionAsset inputActionAsset)
        {
            _inputActionAsset = inputActionAsset;
        }

        public void RegisterResources()
        {
            _newInputProvider = new NewInputProvider(_inputActionAsset);
            _newInputProvider.Enable();
            GameWorld.SetResource(_newInputProvider);
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new PlayerInputSystem(), _inputSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
            _newInputProvider.Disable();
        }
    }
}
