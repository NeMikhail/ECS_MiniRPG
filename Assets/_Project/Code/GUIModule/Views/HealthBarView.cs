using System;
using System.Reflection;
using ECSMiniRPG.GameplayModule.Components;
using MAEngine.UI.CustomElements;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace ECSMiniRPG.GUIModule.Views
{
    [RequireComponent(typeof(Billboard))]
    public sealed class HealthBarView : MonoBehaviour
    {
        private static readonly string _barName = "hp-bar";
        private static readonly string _documentError = "Health bar UIDocument is not assigned.";
        private static readonly string _barError = "Health bar element is not found in UIDocument.";
        private static readonly FieldInfo _worldSpaceWidthField = typeof(UIDocument).GetField("m_WorldSpaceWidth", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo _worldSpaceHeightField = typeof(UIDocument).GetField("m_WorldSpaceHeight", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly float _minMaxHP = 1f;
        private static readonly float _lowValue = 0f;
        private static readonly float _highValue = 100f;
        private static readonly float _defaultDocumentWidth = 128f;
        private static readonly float _defaultDocumentHeight = 12.8f;

        [FormerlySerializedAs("_legacyDocument")]
        [SerializeField] private UIDocument _document;
        [SerializeField] private Billboard _billboard;
        [SerializeField] private Color _fillColor = new Color(0.9f, 0.08f, 0.06f, 1f);
        [SerializeField] private Color _backgroundColor = new Color(0.05f, 0.05f, 0.05f, 0.72f);

        private Health _health;
        private HorizontalProgressBar _bar;
        private bool _hasHealth;
        private IDisposable _currentValueDisposable;
        private IDisposable _maxValueDisposable;

        public void Bind(Health health)
        {
            _health = health;
            _hasHealth = true;
            EnsureDocument();
            EnsureBar();

            if (_bar != null)
            {
                SubscribeToHealth();
                UpdateProgress();
            }
        }

        public void SetCamera(Camera camera)
        {
            EnsureBillboard();

            if (_billboard != null)
            {
                _billboard.SetCamera(camera);
            }
        }

        private void Awake()
        {
            EnsureDocument();
            EnsureBillboard();
            EnsureBar();
        }

        private void OnEnable()
        {
            EnsureDocument();
            EnsureBar();

            if (_bar != null && _hasHealth && _health._currentValue != null && _health._maxValue != null)
            {
                SubscribeToHealth();
                UpdateProgress();
            }
        }

        private void OnDisable()
        {
            DisposeSubscriptions();
        }

        private void OnDestroy()
        {
            DisposeSubscriptions();
        }

        private void Start()
        {
            EnsureBar();

            if (_bar != null && _hasHealth && _health._currentValue != null && _health._maxValue != null)
            {
                SubscribeToHealth();
                UpdateProgress();
            }
        }

        private void OnValidate()
        {
            EnsureDocument();
            EnsureBillboard();
        }

        private void EnsureDocument()
        {
            if (_document == null)
            {
                _document = GetComponent<UIDocument>();
            }

            if (_document == null)
            {
                Debug.LogError(_documentError, this);
            }
        }

        private void EnsureBillboard()
        {
            if (_billboard == null)
            {
                _billboard = GetComponent<Billboard>();
            }
        }

        private void EnsureBar()
        {
            if (_bar != null)
            {
                return;
            }

            if (_document == null)
            {
                return;
            }

            VisualElement root = _document.rootVisualElement;
            if (root == null)
            {
                return;
            }

            _bar = root.Q<HorizontalProgressBar>(_barName);
            if (_bar == null)
            {
                Debug.LogError(_barError, this);
                return;
            }

            _bar.LowValue = _lowValue;
            _bar.HighValue = _highValue;
            ApplyBarSettings();
        }

        private void ApplyBarSettings()
        {
            if (_bar == null)
            {
                return;
            }

            float width = GetDocumentFloatValue(_worldSpaceWidthField, _defaultDocumentWidth);
            float height = GetDocumentFloatValue(_worldSpaceHeightField, _defaultDocumentHeight);
            _bar.style.width = width;
            _bar.style.height = height;
            _bar.FillColor = _fillColor;
            _bar.BackgroundColor = _backgroundColor;
        }

        private float GetDocumentFloatValue(FieldInfo field, float defaultValue)
        {
            if (_document == null || field == null)
            {
                return defaultValue;
            }

            var value = field.GetValue(_document);
            return value is float floatValue && floatValue > 0f ? floatValue : defaultValue;
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
            if (_bar == null || !_hasHealth || _health._currentValue == null || _health._maxValue == null)
            {
                return;
            }

            float maxHP = Mathf.Max(_health._maxValue.Value, _minMaxHP);
            float progress = Mathf.Clamp01(_health._currentValue.Value / maxHP) * _highValue;
            _bar.Progress = progress;
        }
    }
}


