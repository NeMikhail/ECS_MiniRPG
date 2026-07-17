using ECSMiniRPG.LocationModule.Resources.Events;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.DebugTools
{
    public sealed class LocationDebug : MonoBehaviour
    {
        private EventReceiver<GameWorldType, ResourceGatherStartedEvent> _resourceGatherStartedReceiver;
        private EventReceiver<GameWorldType, ResourceGatherFailedEvent> _resourceGatherFailedReceiver;
        private bool _hasReceiver;

        private void OnEnable()
        {
            TryRegisterReceivers();
        }

        private void Update()
        {
            TryRegisterReceivers();

            if (_hasReceiver)
            {
                ProcessResourceGatherStartedEvents();
                ProcessResourceGatherFailedEvents();
            }
        }

        private void OnDisable()
        {
            if (_hasReceiver && GameWorld.IsWorldInitialized)
            {
                GameWorld.DeleteEventReceiver(ref _resourceGatherStartedReceiver);
                GameWorld.DeleteEventReceiver(ref _resourceGatherFailedReceiver);
            }

            _hasReceiver = false;
        }

        private void TryRegisterReceivers()
        {
            if (!_hasReceiver && Application.isPlaying && GameWorld.IsWorldInitialized)
            {
                _resourceGatherStartedReceiver = GameWorld.RegisterEventReceiver<ResourceGatherStartedEvent>();
                _resourceGatherFailedReceiver = GameWorld.RegisterEventReceiver<ResourceGatherFailedEvent>();
                _hasReceiver = true;
            }
        }

        private void ProcessResourceGatherStartedEvents()
        {
            foreach (var resourceGatherStartedEvent in _resourceGatherStartedReceiver)
            {
                PrintResourceGatherStartedEvent(resourceGatherStartedEvent.Value);
            }
        }

        private void ProcessResourceGatherFailedEvents()
        {
            foreach (var resourceGatherFailedEvent in _resourceGatherFailedReceiver)
            {
                PrintResourceGatherFailedEvent(resourceGatherFailedEvent.Value);
            }
        }

        private void PrintResourceGatherStartedEvent(ResourceGatherStartedEvent resourceGatherStartedEvent)
        {
            var resourceId = resourceGatherStartedEvent._resource != null ? resourceGatherStartedEvent._resource.Id : "None";
            Debug.Log($"Resource gather started. Resource: {resourceId}. Duration: {resourceGatherStartedEvent._duration}. Range: {resourceGatherStartedEvent._range}.");
        }

        private void PrintResourceGatherFailedEvent(ResourceGatherFailedEvent resourceGatherFailedEvent)
        {
            var resourceId = resourceGatherFailedEvent._resource != null ? resourceGatherFailedEvent._resource.Id : "None";
            var requiredToolId = resourceGatherFailedEvent._requiredTool != null ? resourceGatherFailedEvent._requiredTool.Id : "None";
            Debug.LogWarning($"Resource gather failed. Resource: {resourceId}. Reason: {resourceGatherFailedEvent._reason}. Required tool: {requiredToolId}.");
        }
    }
}
