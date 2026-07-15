using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InputSystem.Systems
{
    public struct PlayerInputSystem : ISystem
    {
        public void Update()
        {
            ref var newInputProvider = ref GameWorld.GetResource<NewInputProvider>();
            var moveDirection = newInputProvider.MoveDirection;

            foreach (var entity in GameWorld.Query<All<PlayerTag, PlayerInputData>>().Entities())
            {
                ref var inputData = ref entity.Ref<PlayerInputData>();
                inputData._moveDirection = moveDirection;
            }
        }
    }
}
