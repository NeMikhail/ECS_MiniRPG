using ECSMiniRPG.Core;
using UnityEngine;

namespace ECSMiniRPG.GUIModule
{
    public sealed class GUIView : MonoBehaviour, IView
    {
        [SerializeField] private PauseMenuView _pauseMenuView;

        public bool IsInitialized { get; private set; }
        public PauseMenuView PauseMenuView => _pauseMenuView;

        public void Initialize()
        {
            if (!IsInitialized && _pauseMenuView != null)
            {
                _pauseMenuView.Initialize();
                IsInitialized = _pauseMenuView.IsInitialized;
            }

            if (_pauseMenuView == null)
            {
                Debug.LogError("PauseMenuView is not assigned in GUIView.");
            }
        }
    }
}
