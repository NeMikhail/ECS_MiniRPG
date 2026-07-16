using System.Collections;
using MAEngine.UI.CustomElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MAEngine.UI.Testing
{
    [RequireComponent(typeof(UIDocument))]
    public class VerticalProgressBarTester : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool _runTestOnStart = true;
        [SerializeField] private float _delayBetweenActions = 1.5f;

        [Header("UI Document")]
        [SerializeField] private UIDocument _uiDocument;

        private VerticalProgressBar _progressBar1;
        private VerticalProgressBar _progressBar2;
        private VerticalProgressBar _progressBar3;
        private VerticalProgressBar _codeBar1;
        private VerticalProgressBar _codeBar2;
        private VerticalProgressBar _codeBar3;

        private void OnValidate()
        {
            if (_uiDocument == null)
                _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            InitializeUI();

            if (_runTestOnStart)
                StartCoroutine(RunProgressBarTest());
        }

        private void InitializeUI()
        {
            if (_uiDocument == null)
            {
                Debug.LogError("UIDocument is not assigned!");
                return;
            }

            var root = _uiDocument.rootVisualElement;

            _progressBar1 = root.Q<VerticalProgressBar>("test-pb-1");
            _progressBar2 = root.Q<VerticalProgressBar>("test-pb-2");
            _progressBar3 = root.Q<VerticalProgressBar>("test-pb-3");

            if (_progressBar1 == null || _progressBar2 == null || _progressBar3 == null)
            {
                Debug.LogError("Could not find progress bars in UXML!");
                return;
            }

            _progressBar1.FillColor = new Color(0.2f, 0.8f, 0.4f, 1f);
            _progressBar2.FillColor = new Color(0.2f, 0.5f, 1f, 1f);
            _progressBar3.FillColor = new Color(1f, 0.4f, 0.2f, 1f);

            var line2 = root.Q<VisualElement>("Line2");

            if (line2 == null)
            {
                Debug.LogError("Could not find Line2 in UXML!");
                return;
            }

            _codeBar1 = CreateBar(
                fillColor: new Color(0.8f, 0.2f, 1f, 1f),
                bgColor: new Color(0.2f, 0.2f, 0.2f, 0.4f),
                width: 24f,
                height: 120f,
                sectorCount: 5,
                sectorGap: 4f,
                label: "Sectored (5)",
                labelColor: new Color(0.8f, 0.2f, 1f)
            );
            line2.Add(CreateWrapper(_codeBar1, "Sectored (5)", new Color(0.8f, 0.2f, 1f)));

            _codeBar2 = CreateBar(
                fillColor: new Color(0.2f, 0.9f, 0.9f, 1f),
                bgColor: new Color(0.2f, 0.2f, 0.2f, 0.4f),
                width: 24f,
                height: 120f,
                sectorCount: 8,
                sectorGap: 3f,
                label: "Sectored (8)",
                labelColor: new Color(0.2f, 0.9f, 0.9f)
            );
            line2.Add(CreateWrapper(_codeBar2, "Sectored (8)", new Color(0.2f, 0.9f, 0.9f)));

            _codeBar3 = CreateBar(
                fillColor: new Color(1f, 0.6f, 0.1f, 1f),
                bgColor: new Color(0.2f, 0.2f, 0.2f, 0.4f),
                width: 24f,
                height: 120f,
                sectorCount: 4,
                sectorGap: 5f,
                label: "Discrete (4)",
                labelColor: new Color(1f, 0.6f, 0.1f)
            );
            _codeBar3.DiscreteProgress = true;
            line2.Add(CreateWrapper(_codeBar3, "Discrete (4)", new Color(1f, 0.6f, 0.1f)));

            Debug.Log("=== VERTICAL PROGRESS BAR UI INITIALIZED ===");
        }

        private VerticalProgressBar CreateBar(Color fillColor, Color bgColor, float width, float height,
            int sectorCount, float sectorGap, string label, Color labelColor)
        {
            return new VerticalProgressBar
            {
                Progress = 0f,
                FillColor = fillColor,
                BackgroundColor = bgColor,
                BarWidth = width,
                BarHeight = height,
                SectorCount = sectorCount,
                SectorGap = sectorGap
            };
        }

        private VisualElement CreateWrapper(VerticalProgressBar bar, string title, Color titleColor)
        {
            var wrapper = new VisualElement();
            wrapper.style.flexDirection = FlexDirection.Column;
            wrapper.style.alignItems = Align.Center;
            wrapper.style.justifyContent = Justify.FlexEnd;
            wrapper.style.marginLeft = 20;
            wrapper.style.marginRight = 20;

            var label = new Label(title);
            label.style.fontSize = 14;
            label.style.color = titleColor;
            label.style.marginTop = 8;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;

            wrapper.Add(bar);
            wrapper.Add(label);

            return wrapper;
        }

        private IEnumerator RunProgressBarTest()
        {
            Debug.Log("=== VERTICAL PROGRESS BAR TEST STARTED ===");
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 1: Fill Line1 bars to 25%
            Debug.Log("TEST 1: Fill Line1 bars to 25%");
            SetAllLine1Bars(0.25f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 2: Fill Line1 bars to 50%
            Debug.Log("TEST 2: Fill Line1 bars to 50%");
            SetAllLine1Bars(0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 3: Fill Line1 bars to 75%
            Debug.Log("TEST 3: Fill Line1 bars to 75%");
            SetAllLine1Bars(0.75f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 4: Fill Line1 bars to 100%
            Debug.Log("TEST 4: Fill Line1 bars to 100%");
            SetAllLine1Bars(1.0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 5: Reset Line1 bars
            Debug.Log("TEST 5: Reset Line1 bars");
            SetAllLine1Bars(0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 6: Animated fill Line1 bars with different speeds
            Debug.Log("TEST 6: Animated fill Line1 bars with different speeds");
            _progressBar1.SetProgressAnimated(1.0f, 3.0f);
            _progressBar2.SetProgressAnimated(1.0f, 2.0f);
            _progressBar3.SetProgressAnimated(1.0f, 4.0f);
            yield return new WaitForSeconds(4.5f);

            // Test 7: Different values per bar
            Debug.Log("TEST 7: Set different values per bar");
            _progressBar1.Progress = 0.3f;
            _progressBar2.Progress = 0.6f;
            _progressBar3.Progress = 0.9f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 8: Change fill colors
            Debug.Log("TEST 8: Change fill colors");
            _progressBar1.FillColor = new Color(1f, 0.9f, 0.1f, 1f);
            _progressBar2.FillColor = new Color(1f, 0.2f, 0.6f, 1f);
            _progressBar3.FillColor = new Color(0.4f, 0.9f, 0.2f, 1f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 9: Change background colors
            Debug.Log("TEST 9: Change background colors");
            _progressBar1.BackgroundColor = new Color(0.5f, 0.3f, 0.1f, 0.5f);
            _progressBar2.BackgroundColor = new Color(0.1f, 0.2f, 0.5f, 0.5f);
            _progressBar3.BackgroundColor = new Color(0.1f, 0.4f, 0.1f, 0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 10: Reset Line1 colors
            Debug.Log("TEST 10: Reset Line1 colors");
            _progressBar1.FillColor = new Color(0.2f, 0.8f, 0.4f, 1f);
            _progressBar1.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            _progressBar2.FillColor = new Color(0.2f, 0.5f, 1f, 1f);
            _progressBar2.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            _progressBar3.FillColor = new Color(1f, 0.4f, 0.2f, 1f);
            _progressBar3.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 11: Change bar dimensions
            Debug.Log("TEST 11: Change bar dimensions");
            _progressBar1.BarWidth = 35f;
            _progressBar2.BarWidth = 15f;
            _progressBar3.BarHeight = 160f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 12: Restore bar dimensions
            Debug.Log("TEST 12: Restore bar dimensions");
            _progressBar1.BarWidth = 20f;
            _progressBar2.BarWidth = 20f;
            _progressBar3.BarHeight = 100f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 13: Code-spawned sectored bars - fill to 50%
            Debug.Log("TEST 13: Code bars - fill to 50%");
            SetAllCodeBars(0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 14: Code bars - fill to 100%
            Debug.Log("TEST 14: Code bars - fill to 100%");
            SetAllCodeBars(1.0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 15: Code bars - animated fill from 0
            Debug.Log("TEST 15: Code bars - animated fill from 0 to 1");
            SetAllCodeBars(0f);
            yield return new WaitForSeconds(0.3f);
            _codeBar1.SetProgressAnimated(1.0f, 2.0f);
            _codeBar2.SetProgressAnimated(1.0f, 2.5f);
            _codeBar3.SetProgressAnimated(1.0f, 1.5f);
            yield return new WaitForSeconds(3.0f);

            // Test 16: Code bars - different values
            Debug.Log("TEST 16: Code bars - different progress values");
            _codeBar1.Progress = 0.4f;
            _codeBar2.Progress = 0.7f;
            _codeBar3.Progress = 0.5f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 17: Toggle discrete on codeBar3
            Debug.Log("TEST 17: Toggle discrete mode on codeBar3");
            _codeBar3.DiscreteProgress = false;
            _codeBar3.Progress = 0.6f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 18: Restore discrete
            Debug.Log("TEST 18: Restore discrete mode");
            _codeBar3.DiscreteProgress = true;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 19: Change sector count dynamically
            Debug.Log("TEST 19: Change codeBar1 sector count to 10");
            _codeBar1.SectorCount = 10;
            _codeBar1.Progress = 0.7f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 20: Change sector gap
            Debug.Log("TEST 20: Change codeBar2 sector gap to 8");
            _codeBar2.SectorGap = 8f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 21: Reset code bars
            Debug.Log("TEST 21: Reset code bars");
            _codeBar1.SectorCount = 5;
            _codeBar1.SectorGap = 4f;
            _codeBar2.SectorGap = 3f;
            SetAllCodeBars(0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 22: Rapid update all bars
            Debug.Log("TEST 22: Rapid update all bars (ping-pong)");
            float elapsed = 0f;
            while (elapsed < 5f)
            {
                float progress = Mathf.PingPong(elapsed * 0.5f, 1f);
                SetAllLine1Bars(progress);
                SetAllCodeBars(progress);
                elapsed += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 23: Final showcase - all bars animated to 100%
            Debug.Log("TEST 23: Final showcase - all bars to 100%");
            SetAllLine1Bars(0f);
            SetAllCodeBars(0f);
            yield return new WaitForSeconds(0.3f);
            _progressBar1.SetProgressAnimated(1.0f, 2.0f);
            _progressBar2.SetProgressAnimated(1.0f, 2.0f);
            _progressBar3.SetProgressAnimated(1.0f, 2.0f);
            _codeBar1.SetProgressAnimated(1.0f, 2.0f);
            _codeBar2.SetProgressAnimated(1.0f, 2.0f);
            _codeBar3.SetProgressAnimated(1.0f, 2.0f);
            yield return new WaitForSeconds(2.5f);

            Debug.Log("=== VERTICAL PROGRESS BAR TEST COMPLETED ===");
        }

        private void SetAllLine1Bars(float progress)
        {
            if (_progressBar1 != null) _progressBar1.Progress = progress;
            if (_progressBar2 != null) _progressBar2.Progress = progress;
            if (_progressBar3 != null) _progressBar3.Progress = progress;
        }

        private void SetAllCodeBars(float progress)
        {
            if (_codeBar1 != null) _codeBar1.Progress = progress;
            if (_codeBar2 != null) _codeBar2.Progress = progress;
            if (_codeBar3 != null) _codeBar3.Progress = progress;
        }

        [ContextMenu("Run Test Now")]
        public void RunTestNow()
        {
            if (_progressBar1 == null)
                InitializeUI();
            StartCoroutine(RunProgressBarTest());
        }

        [ContextMenu("Set All to 25%")]
        public void SetAllTo25()
        {
            SetAllLine1Bars(0.25f);
            SetAllCodeBars(0.25f);
        }

        [ContextMenu("Set All to 50%")]
        public void SetAllTo50()
        {
            SetAllLine1Bars(0.5f);
            SetAllCodeBars(0.5f);
        }

        [ContextMenu("Set All to 75%")]
        public void SetAllTo75()
        {
            SetAllLine1Bars(0.75f);
            SetAllCodeBars(0.75f);
        }

        [ContextMenu("Set All to 100%")]
        public void SetAllTo100()
        {
            SetAllLine1Bars(1.0f);
            SetAllCodeBars(1.0f);
        }

        [ContextMenu("Reset All to 0%")]
        public void ResetAll()
        {
            SetAllLine1Bars(0f);
            SetAllCodeBars(0f);
        }

        [ContextMenu("Animate All to 100%")]
        public void AnimateAllTo100()
        {
            if (_progressBar1 != null) _progressBar1.SetProgressAnimated(1.0f, 2.0f);
            if (_progressBar2 != null) _progressBar2.SetProgressAnimated(1.0f, 2.0f);
            if (_progressBar3 != null) _progressBar3.SetProgressAnimated(1.0f, 2.0f);
            if (_codeBar1 != null) _codeBar1.SetProgressAnimated(1.0f, 2.0f);
            if (_codeBar2 != null) _codeBar2.SetProgressAnimated(1.0f, 2.0f);
            if (_codeBar3 != null) _codeBar3.SetProgressAnimated(1.0f, 2.0f);
        }

        [ContextMenu("Random Colors on Line1")]
        public void RandomColorsLine1()
        {
            if (_progressBar1 != null) _progressBar1.FillColor = new Color(Random.value, Random.value, Random.value, 1f);
            if (_progressBar2 != null) _progressBar2.FillColor = new Color(Random.value, Random.value, Random.value, 1f);
            if (_progressBar3 != null) _progressBar3.FillColor = new Color(Random.value, Random.value, Random.value, 1f);
        }

        [ContextMenu("Toggle Discrete on CodeBar3")]
        public void ToggleDiscreteCodeBar3()
        {
            if (_codeBar3 != null)
            {
                _codeBar3.DiscreteProgress = !_codeBar3.DiscreteProgress;
                Debug.Log($"CodeBar3 discrete mode: {_codeBar3.DiscreteProgress}");
            }
        }

        [ContextMenu("Continuous Update Test")]
        public void ContinuousUpdateTest()
        {
            StartCoroutine(ContinuousUpdate());
        }

        private IEnumerator ContinuousUpdate()
        {
            Debug.Log("Starting continuous update test (10 seconds)");
            float elapsed = 0f;
            while (elapsed < 10f)
            {
                float progress = Mathf.PingPong(elapsed * 0.5f, 1f);
                SetAllLine1Bars(progress);
                SetAllCodeBars(progress);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Continuous update test completed");
        }
    }
}
