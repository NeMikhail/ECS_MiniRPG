using ECSMiniRPG.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECSMiniRPG.MainMenuModule
{
    public sealed class MainMenuView : MonoBehaviour, IView
    {
        private static readonly string _startButtonName = "button-start";
        private static readonly string _settingsButtonName = "button-settings";
        private static readonly string _exitButtonName = "button-exit";
        private static readonly string _settingsPanelName = "settings-panel";
        private static readonly string _closeSettingsButtonName = "button-close-settings";

        [SerializeField] private UIDocument _document;

        public bool IsInitialized { get; private set; }
        public Button StartButton { get; private set; }
        public Button SettingsButton { get; private set; }
        public Button ExitButton { get; private set; }
        public VisualElement SettingsPanel { get; private set; }
        public Button CloseSettingsButton { get; private set; }

        public void Initialize()
        {
            if (!IsInitialized && _document != null)
            {
                StartButton = _document.rootVisualElement.Q<Button>(_startButtonName);
                SettingsButton = _document.rootVisualElement.Q<Button>(_settingsButtonName);
                ExitButton = _document.rootVisualElement.Q<Button>(_exitButtonName);
                SettingsPanel = _document.rootVisualElement.Q<VisualElement>(_settingsPanelName);
                CloseSettingsButton = _document.rootVisualElement.Q<Button>(_closeSettingsButtonName);
                IsInitialized = true;
            }

            if (_document == null)
            {
                Debug.LogError("UIDocument is not assigned in MainMenuView.");
            }
        }

        public void ShowSettings()
        {
            SettingsPanel.style.display = DisplayStyle.Flex;
        }

        public void HideSettings()
        {
            SettingsPanel.style.display = DisplayStyle.None;
        }
    }
}
