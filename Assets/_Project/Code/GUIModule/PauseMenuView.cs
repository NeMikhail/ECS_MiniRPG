using UnityEngine;
using UnityEngine.UIElements;

namespace ECSMiniRPG.GUIModule
{
    public sealed class PauseMenuView : MonoBehaviour
    {
        private static readonly string _pauseMenuName = "pause-menu";
        private static readonly string _resumeButtonName = "button-pause-resume";
        private static readonly string _mainMenuButtonName = "button-pause-main-menu";
        private static readonly string _exitButtonName = "button-pause-exit";

        [SerializeField] private UIDocument _document;

        public bool IsInitialized { get; private set; }
        public VisualElement Panel { get; private set; }
        public Button ResumeButton { get; private set; }
        public Button MainMenuButton { get; private set; }
        public Button ExitButton { get; private set; }

        public void Initialize()
        {
            if (!IsInitialized && _document != null)
            {
                Panel = _document.rootVisualElement.Q<VisualElement>(_pauseMenuName);
                ResumeButton = _document.rootVisualElement.Q<Button>(_resumeButtonName);
                MainMenuButton = _document.rootVisualElement.Q<Button>(_mainMenuButtonName);
                ExitButton = _document.rootVisualElement.Q<Button>(_exitButtonName);
                IsInitialized = Panel != null && ResumeButton != null && MainMenuButton != null && ExitButton != null;
            }

            if (_document == null)
            {
                Debug.LogError("UIDocument is not assigned in PauseMenuView.");
            }

            if (_document != null && !IsInitialized)
            {
                Debug.LogError("PauseMenuView cannot find required UI elements.");
            }
        }
    }
}
