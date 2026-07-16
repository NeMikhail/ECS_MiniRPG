using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ECSMiniRPG.MainMenuModule
{
    public sealed class MainMenuController : IResource
    {
        private static readonly string _gameHubSceneName = "GameHubScene";

        private readonly MainMenuView _view;
        private bool _isInitialized;

        public MainMenuController(MainMenuView view)
        {
            _view = view;
        }

        public void Initialize()
        {
            _view.StartButton.clicked += LoadGameHubScene;
            _view.SettingsButton.clicked += ShowSettings;
            _view.ExitButton.clicked += StopApplication;
            _view.CloseSettingsButton.clicked += HideSettings;
            _view.HideSettings();
            _isInitialized = true;
        }

        public void Destroy()
        {
            if (_isInitialized)
            {
                _view.StartButton.clicked -= LoadGameHubScene;
                _view.SettingsButton.clicked -= ShowSettings;
                _view.ExitButton.clicked -= StopApplication;
                _view.CloseSettingsButton.clicked -= HideSettings;
                _isInitialized = false;
            }
        }

        private void LoadGameHubScene()
        {
            SceneManager.LoadScene(_gameHubSceneName);
        }

        private void ShowSettings()
        {
            _view.ShowSettings();
        }

        private void HideSettings()
        {
            _view.HideSettings();
        }

        private void StopApplication()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
