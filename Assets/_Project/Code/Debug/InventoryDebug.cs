using ECSMiniRPG.InventoryModule.Drop;
using ECSMiniRPG.InventoryModule.Events;
using ECSMiniRPG.InventoryModule.Pickup;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.DebugTools
{
    public sealed class InventoryDebug : MonoBehaviour
    {
        private EventReceiver<GameWorldType, InventoryPickupEvent> _pickupReceiver;
        private EventReceiver<GameWorldType, InventoryDropEvent> _dropReceiver;
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
                ProcessPickupEvents();
                ProcessDropEvents();
            }
        }

        private void OnDisable()
        {
            if (_hasReceiver && GameWorld.IsWorldInitialized)
            {
                GameWorld.DeleteEventReceiver(ref _pickupReceiver);
                GameWorld.DeleteEventReceiver(ref _dropReceiver);
            }

            _hasReceiver = false;
        }

        private void TryRegisterReceivers()
        {
            if (!_hasReceiver && Application.isPlaying && GameWorld.IsWorldInitialized)
            {
                _pickupReceiver = GameWorld.RegisterEventReceiver<InventoryPickupEvent>();
                _dropReceiver = GameWorld.RegisterEventReceiver<InventoryDropEvent>();
                _hasReceiver = true;
            }
        }

        private void ProcessPickupEvents()
        {
            foreach (var pickupEvent in _pickupReceiver)
            {
                PrintPickupEvent(pickupEvent.Value);
            }
        }

        private void ProcessDropEvents()
        {
            foreach (var dropEvent in _dropReceiver)
            {
                PrintDropEvent(dropEvent.Value);
            }
        }

        private void PrintPickupEvent(InventoryPickupEvent pickupEvent)
        {
            if (pickupEvent._resultType == InventoryPickupResultType.Success)
            {
                Debug.Log($"Pickup succeeded. Item: {pickupEvent._item.Title}. Added: {pickupEvent._addedCount}.");
            }

            if (pickupEvent._resultType == InventoryPickupResultType.Partial)
            {
                Debug.Log($"Pickup partially succeeded. Item: {pickupEvent._item.Title}. Added: {pickupEvent._addedCount}. Remaining: {pickupEvent._remainingCount}.");
            }

            if (pickupEvent._resultType == InventoryPickupResultType.Failed)
            {
                Debug.Log($"Pickup failed. Item: {pickupEvent._item.Title}. Requested: {pickupEvent._requestedCount}.");
            }
        }

        private void PrintDropEvent(InventoryDropEvent dropEvent)
        {
            var itemTitle = dropEvent._item != null ? dropEvent._item.Title : "None";

            if (dropEvent._resultType == InventoryDropResultType.Success)
            {
                Debug.Log($"Drop succeeded. Item: {itemTitle}. Dropped: {dropEvent._droppedCount}.");
            }

            if (dropEvent._resultType == InventoryDropResultType.Failed && dropEvent._failReason == InventoryDropFailReason.MissingLootConfig)
            {
                Debug.LogWarning($"Drop forbidden. Item: {itemTitle}. Reason: {dropEvent._failReason}.");
            }

            if (dropEvent._resultType == InventoryDropResultType.Failed && dropEvent._failReason != InventoryDropFailReason.MissingLootConfig)
            {
                Debug.Log($"Drop failed. Item: {itemTitle}. Requested: {dropEvent._requestedCount}. Reason: {dropEvent._failReason}.");
            }
        }
    }
}