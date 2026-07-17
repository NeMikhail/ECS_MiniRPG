using ECSMiniRPG.GameplayModule.LongActions;
using ECSMiniRPG.GameplayModule.LongActions.Events;
using ECSMiniRPG.GameplayModule.LongActions.Components;
using ECSMiniRPG.InputSystem;
using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InteractionModule.Systems
{
    public struct InteractionInputSystem : ISystem
    {
        public void Update()
        {
            ref var inputProvider = ref GameWorld.GetResource<NewInputProvider>();

            if (inputProvider.WasInteractPressedThisFrame())
            {
                EnqueuePlayerInteractions();
            }
        }

        private void EnqueuePlayerInteractions()
        {
            ref var commandQueue = ref GameWorld.GetResource<InteractionCommandQueue>();

            foreach (var entity in GameWorld.Query<All<PlayerTag>>().Entities())
            {
                if (!entity.Has<LongActionComponent>())
                {
                    commandQueue.Enqueue(entity);
                }

                GameWorld.SendEvent(new LongActionInterruptEvent
                {
                    _owner = entity,
                    _reason = LongActionCancelReason.Action
                });
            }
        }
    }
}
