using ECSMiniRPG.Core;

namespace ECSMiniRPG.MainMenuModule
{
    public sealed class MainMenuModuleFactory : IEcsModuleFactory
    {
        private readonly ViewsProvider _viewsProvider;
        private MainMenuController _mainMenuController;

        public MainMenuModuleFactory(ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
        }

        public void RegisterResources()
        {
            var view = _viewsProvider.GetView<MainMenuView>();

            if (view != null)
            {
                view.Initialize();
            }

            if (view != null && view.IsInitialized)
            {
                _mainMenuController = new MainMenuController(view);
                _mainMenuController.Initialize();
                GameWorld.SetResource(_mainMenuController);
            }
        }

        public void RegisterUpdateSystems()
        {
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
            if (_mainMenuController != null)
            {
                _mainMenuController.Destroy();
            }
        }
    }
}
