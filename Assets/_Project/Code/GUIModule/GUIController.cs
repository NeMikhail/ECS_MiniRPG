using System.Collections.Generic;
using ECSMiniRPG.GUIModule.Elements;
using ECSMiniRPG.InputSystem;
using FFS.Libraries.StaticEcs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ECSMiniRPG.GUIModule
{
    public sealed class GUIController : IResource
    {
        private static readonly string _mainMenuSceneName = "Menu";

        private readonly GUIView _view;
        private readonly NewInputProvider _inputProvider;
        private readonly List<GUIPanel> _panels = new List<GUIPanel>();
        private PauseMenuView _pauseMenuView;
        private PauseMenuElement _pauseMenuElement;
        private bool _isInitialized;

        public GUIController(GUIView view, NewInputProvider inputProvider)
        {
            _view = view;
            _inputProvider = inputProvider;
        }

        public void Initialize()
        {
            _pauseMenuView = _view.PauseMenuView;
            _pauseMenuElement = new PauseMenuElement(_pauseMenuView.Panel);
            _panels.Add(_pauseMenuElement);
            _pauseMenuView.ResumeButton.clicked += ClosePauseMenu;
            _pauseMenuView.MainMenuButton.clicked += LoadMainMenu;
            _pauseMenuView.ExitButton.clicked += StopApplication;
            UpdatePlayerInputBlock();
            _isInitialized = true;
        }

        public void Update()
        {
            if (_inputProvider.WasPausePressedThisFrame())
            {
                _pauseMenuElement.Toggle();
                UpdatePlayerInputBlock();
            }
        }

        public void Destroy()
        {
            if (_isInitialized)
            {
                _pauseMenuView.ResumeButton.clicked -= ClosePauseMenu;
                _pauseMenuView.MainMenuButton.clicked -= LoadMainMenu;
                _pauseMenuView.ExitButton.clicked -= StopApplication;
                _inputProvider.SetPlayerInputBlocked(false);
                _isInitialized = false;
            }
        }

        private void ClosePauseMenu()
        {
            _pauseMenuElement.Close();
            UpdatePlayerInputBlock();
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        private void StopApplication()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void UpdatePlayerInputBlock()
        {
            var isPlayerInputBlocked = false;

            for (var i = 0; i < _panels.Count; i++)
            {
                if (_panels[i].IsOpen && _panels[i].IsPlayerInputBlocked)
                {
                    isPlayerInputBlocked = true;
                }
            }

            _inputProvider.SetPlayerInputBlocked(isPlayerInputBlocked);
        }
    }
}
