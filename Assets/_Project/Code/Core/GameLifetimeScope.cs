using System.Collections.Generic;
using ECSMiniRPG.ContentManagement;
using ECSMiniRPG.GameplayModule;
using ECSMiniRPG.GUIModule;
using ECSMiniRPG.InputSystem;
using ECSMiniRPG.InteractionModule;
using ECSMiniRPG.InventoryModule;
using ECSMiniRPG.LocationModule;
using ECSMiniRPG.MainMenuModule;
using ECSMiniRPG.PlayerModule;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace ECSMiniRPG.Core
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameBootstrap _gameBootstrap;
        [SerializeField] private GameConfigs _gameConfigs;
        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private ViewsProvider _viewsProvider;
        [SerializeField] private List<EcsModuleInstallerConfig> _moduleInstallerConfigs;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_gameConfigs);
            builder.RegisterInstance(_inputActionAsset);
            builder.RegisterComponent(_gameBootstrap);
            builder.RegisterComponent(_viewsProvider);
            builder.Register<ContentManagementSystem>(Lifetime.Singleton).AsSelf();
            builder.Register<CoreModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<InputModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<LocationModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<PlayerModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<MainMenuModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<GUIModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<InventoryModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<GameplayModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register<InteractionModuleFactory>(Lifetime.Singleton).AsSelf();
            builder.Register(c => BuildModuleFactories(c), Lifetime.Singleton);
        }

        private IEcsModuleFactory[] BuildModuleFactories(IObjectResolver resolver)
        {
            var moduleFactories = new IEcsModuleFactory[CalculateEnabledModulesCount()];
            var moduleIndex = 0;

            for (var i = 0; i < _moduleInstallerConfigs.Count; i++)
            {
                var config = _moduleInstallerConfigs[i];
                if (config.IsEnabled)
                {
                    moduleFactories[moduleIndex] = ResolveModuleFactory(resolver, config.ModuleType);
                    moduleIndex++;
                }
            }

            return moduleFactories;
        }

        private int CalculateEnabledModulesCount()
        {
            var count = 0;

            for (var i = 0; i < _moduleInstallerConfigs.Count; i++)
            {
                if (_moduleInstallerConfigs[i].IsEnabled)
                {
                    count++;
                }
            }

            return count;
        }

        private IEcsModuleFactory ResolveModuleFactory(IObjectResolver resolver, EcsModuleType moduleType)
        {
            IEcsModuleFactory moduleFactory = resolver.Resolve<CoreModuleFactory>();

            if (moduleType == EcsModuleType.Input)
            {
                moduleFactory = resolver.Resolve<InputModuleFactory>();
            }

            if (moduleType == EcsModuleType.Player)
            {
                moduleFactory = resolver.Resolve<PlayerModuleFactory>();
            }

            if (moduleType == EcsModuleType.Location)
            {
                moduleFactory = resolver.Resolve<LocationModuleFactory>();
            }

            if (moduleType == EcsModuleType.MainMenu)
            {
                moduleFactory = resolver.Resolve<MainMenuModuleFactory>();
            }

            if (moduleType == EcsModuleType.GUI)
            {
                moduleFactory = resolver.Resolve<GUIModuleFactory>();
            }

            if (moduleType == EcsModuleType.Inventory)
            {
                moduleFactory = resolver.Resolve<InventoryModuleFactory>();
            }

            if (moduleType == EcsModuleType.Gameplay)
            {
                moduleFactory = resolver.Resolve<GameplayModuleFactory>();
            }

            if (moduleType == EcsModuleType.Interaction)
            {
                moduleFactory = resolver.Resolve<InteractionModuleFactory>();
            }

            return moduleFactory;
        }
    }
}

