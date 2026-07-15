using System.Collections.Generic;
using ECSMiniRPG.ContentManagement;
using ECSMiniRPG.InputSystem;
using ECSMiniRPG.LocationModule;
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

            return moduleFactory;
        }
    }
}
