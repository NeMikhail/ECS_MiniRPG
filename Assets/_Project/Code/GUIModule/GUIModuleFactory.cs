using ECSMiniRPG.Core;
using ECSMiniRPG.GUIModule.Systems;
using ECSMiniRPG.InputSystem;

namespace ECSMiniRPG.GUIModule
{
    public sealed class GUIModuleFactory : IEcsModuleFactory
    {
        private static readonly short _guiSystemOrder = 0;
        private static readonly short _healthBarBindingSystemOrder = 50;

        private readonly ViewsProvider _viewsProvider;
        private GUIController _guiController;

        public GUIModuleFactory(ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
        }

        public void RegisterResources()
        {
            var view = _viewsProvider.GetView<GUIView>();

            if (view != null)
            {
                view.Initialize();
            }

            if (view != null && view.IsInitialized)
            {
                ref var inputProvider = ref GameWorld.GetResource<NewInputProvider>();
                _guiController = new GUIController(view, inputProvider);
                _guiController.Initialize();
                GameWorld.SetResource(_guiController);
            }
        }

        public void RegisterUpdateSystems()
        {
            if (_guiController != null)
            {
                GameSystems.Add(new GUISystem(), _guiSystemOrder);
            }

            GameSystems.Add(new HealthBarBindingSystem(), _healthBarBindingSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
            if (_guiController != null)
            {
                _guiController.Destroy();
            }
        }
    }
}