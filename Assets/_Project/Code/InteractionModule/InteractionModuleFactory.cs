using ECSMiniRPG.Core;
using ECSMiniRPG.InteractionModule.Systems;

namespace ECSMiniRPG.InteractionModule
{
    public sealed class InteractionModuleFactory : IEcsModuleFactory
    {
        private static readonly short _sceneBindingSystemOrder = 5;
        private static readonly short _inputSystemOrder = 10;
        private static readonly short _targetSearchSystemOrder = 20;
        private static readonly short _commandSystemOrder = 30;

        public void RegisterResources()
        {
            GameWorld.SetResource(new InteractionCommandQueue());
        }

        public void RegisterUpdateSystems()
        {
            GameSystems.Add(new InteractionSceneBindingSystem(), _sceneBindingSystemOrder);
            GameSystems.Add(new InteractionInputSystem(), _inputSystemOrder);
            GameSystems.Add(new InteractionTargetSearchSystem(), _targetSearchSystemOrder);
            GameSystems.Add(new InteractionCommandSystem(), _commandSystemOrder);
        }

        public void RegisterFixedSystems()
        {
        }

        public void Destroy()
        {
        }
    }
}
