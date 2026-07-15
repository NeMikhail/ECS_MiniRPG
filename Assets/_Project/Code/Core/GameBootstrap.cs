using ECSMiniRPG.Core;
using UnityEngine;
using VContainer;

namespace ECSMiniRPG
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        private IEcsModuleFactory[] _moduleFactories;
        private bool _isInitialized;

        [Inject]
        public void Construct(IEcsModuleFactory[] moduleFactories)
        {
            _moduleFactories = moduleFactories;
        }

        private void Awake()
        {
            CreateWorld();
            CreatePipelines();
            RegisterModuleResources();
            RegisterModuleSystems();
            InitializePipelines();

            _isInitialized = true;
        }

        private void Update()
        {
            if (_isInitialized)
            {
                GameSystems.Update();
            }
        }

        private void FixedUpdate()
        {
            if (_isInitialized)
            {
                GameFixedSystems.Update();
            }
        }

        private void LateUpdate()
        {
            if (_isInitialized)
            {
                GameWorld.Tick();
            }
        }

        private void OnDestroy()
        {
            if (_isInitialized)
            {
                DestroyPipelines();
                DestroyModuleFactories();
                GameWorld.Destroy();
                _isInitialized = false;
            }
        }

        private void CreateWorld()
        {
            GameWorld.Create();
            GameWorld.Types().RegisterAll(typeof(GameWorldType).Assembly);
            GameWorld.Initialize();
        }

        private void CreatePipelines()
        {
            GameSystems.Create();
            GameFixedSystems.Create();
        }

        private void RegisterModuleResources()
        {
            for (var i = 0; i < _moduleFactories.Length; i++)
            {
                _moduleFactories[i].RegisterResources();
            }
        }

        private void RegisterModuleSystems()
        {
            for (var i = 0; i < _moduleFactories.Length; i++)
            {
                _moduleFactories[i].RegisterUpdateSystems();
                _moduleFactories[i].RegisterFixedSystems();
            }
        }

        private void InitializePipelines()
        {
            GameSystems.Initialize();
            GameFixedSystems.Initialize();
        }

        private void DestroyPipelines()
        {
            GameFixedSystems.Destroy();
            GameSystems.Destroy();
        }

        private void DestroyModuleFactories()
        {
            for (var i = _moduleFactories.Length - 1; i >= 0; i--)
            {
                _moduleFactories[i].Destroy();
            }
        }
    }
}
