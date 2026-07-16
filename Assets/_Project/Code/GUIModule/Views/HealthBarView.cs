using System;
using ECSMiniRPG.GameplayModule.Components;
using MAEngine.UI.CustomElements;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECSMiniRPG.GUIModule.Views
{
    public sealed class HealthBarView : MonoBehaviour
    {
        private static readonly string _barName = "hp-bar";
        private static readonly string _documentError = "Health bar UIDocument is not assigned.";
        private static readonly string _barError = "Health bar element is not found in UIDocument.";
        private static readonly float _defaultWidth = 100f;
        private static readonly float _defaultHeight = 12f;
        private static readonly float _defaultLowValue = 0f;
        private static readonly float _defaultHighValue = 100f;
        private static readonly Color _defaultFillColor = new Color(0.75f, 0.08f, 0.08f, 1f);
        private static readonly Color _defaultBackgroundColor = new Color(0.08f, 0.08f, 0.08f, 0.75f);

        [SerializeField] private UIDocument _document;
        [SerializeField] private Billboard _billboard;
        [SerializeField] private float _width = _defaultWidth;
        [SerializeField] private float _height = _defaultHeight;
        [SerializeField] private Color _fillColor = _defaultFillColor;
        [SerializeField] private Color _backgroundColor = _defaultBackgroundColor;

        private HorizontalProgressBar _bar;
        private Health _health;
        private IDisposable _currentValueDisposable;
        private IDisposable _maxValueDisposable;
        private bool _isBound;

        public void Bind(Health health)
        {
            _health = health;
            _isBound = TryInitializeBar();

            if (_isBound)
            {
                SubscribeToHealth();
                UpdateProgress();
            }
        }

        public void SetCamera(Camera camera)
        {
            if (_billboard != null)
            {
                _billboard.SetCamera(camera);
            }
        }

        private void OnEnable()
        {
            if (_isBound)
            {
                SubscribeToHealth();
            }
        }

        private void OnDisable()
        {
            DisposeSubscriptions();
        }

        private bool TryInitializeBar()
        {
            var hasBar = false;

            if (_document == null)
            {
                Debug.LogError(_documentError);
            }

            if (_document != null && _bar == null)
            {
                _bar = _document.rootVisualElement.Q<HorizontalProgressBar>(_barName);
            }

            if (_bar == null && _document != null)
            {
                Debug.LogError(_barError);
            }

            if (_bar != null)
            {
                ApplyBarSettings();
                hasBar = true;
            }

            return hasBar;
        }

        private void ApplyBarSettings()
        {
            _bar.LowValue = _defaultLowValue;
            _bar.HighValue = _defaultHighValue;
            _bar.FillColor = _fillColor;
            _bar.BackgroundColor = _backgroundColor;
            _bar.style.width = _width;
            _bar.style.height = _height;
        }

        private void SubscribeToHealth()
        {
            DisposeSubscriptions();
            _currentValueDisposable = _health._currentValue.Subscribe(_ => UpdateProgress());
            _maxValueDisposable = _health._maxValue.Subscribe(_ => UpdateProgress());
        }

        private void DisposeSubscriptions()
        {
            if (_currentValueDisposable != null)
            {
                _currentValueDisposable.Dispose();
            }

            if (_maxValueDisposable != null)
            {
                _maxValueDisposable.Dispose();
            }

            _currentValueDisposable = null;
            _maxValueDisposable = null;
        }

        private void UpdateProgress()
        {
            var progress = _defaultLowValue;

            if (_health._maxValue.Value > 0f)
            {
                progress = _health._currentValue.Value / _health._maxValue.Value * _defaultHighValue;
            }

            _bar.Progress = progress;
        }
    }
}