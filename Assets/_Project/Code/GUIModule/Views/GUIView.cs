using ECSMiniRPG.Core;
using UnityEngine;

namespace ECSMiniRPG.GUIModule
{
    public sealed class GUIView : MonoBehaviour, IView
    {
        [SerializeField] private PauseMenuView _pauseMenuView;
        [SerializeField] private InventoryView _inventoryView;

        public bool IsInitialized { get; private set; }
        public PauseMenuView PauseMenuView => _pauseMenuView;
        public InventoryView InventoryView => _inventoryView;

        public void Initialize()
        {
            if (!IsInitialized && _pauseMenuView != null && _inventoryView != null)
            {
                _pauseMenuView.Initialize();
                _inventoryView.Initialize();
                IsInitialized = _pauseMenuView.IsInitialized && _inventoryView.IsInitialized;
            }

            if (_pauseMenuView == null)
            {
                Debug.LogError("PauseMenuView is not assigned in GUIView.");
            }

            if (_inventoryView == null)
            {
                Debug.LogError("InventoryView is not assigned in GUIView.");
            }
        }
    }
}
