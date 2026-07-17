using ECSMiniRPG.InteractionModule.Events;
using ECSMiniRPG.LocationModule.Resources.Events;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.LocationModule.Resources.Systems
{
    public struct ResourceGatherInteractionFailureSystem : ISystem
    {
        private EventReceiver<GameWorldType, InteractionFailedEvent> _interactionFailedReceiver;

        public void Init()
        {
            _interactionFailedReceiver = GameWorld.RegisterEventReceiver<InteractionFailedEvent>();
        }

        public void Update()
        {
            foreach (var interactionFailedEvent in _interactionFailedReceiver)
            {
                Process(interactionFailedEvent.Value);
            }
        }

        public void Destroy()
        {
            GameWorld.DeleteEventReceiver(ref _interactionFailedReceiver);
        }

        private void Process(InteractionFailedEvent interactionFailedEvent)
        {
            if (interactionFailedEvent._reason == InteractionFailReason.NoTarget)
            {
                GameWorld.SendEvent(new ResourceGatherFailedEvent
                {
                    _actor = interactionFailedEvent._actor,
                    _target = default,
                    _resource = null,
                    _requiredTool = null,
                    _reason = ResourceGatherFailReason.NoNearbyResource
                });
            }
        }
    }
}
