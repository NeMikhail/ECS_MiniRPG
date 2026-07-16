using System.Collections;
using MAEngine.UI.CustomElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MAEngine.UI.Testing
{
    [RequireComponent(typeof(UIDocument))]
    public class CircularProgressBarTester : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool _runTestOnStart = true;
        [SerializeField] private float _delayBetweenActions = 1.5f;

        [Header("UI Document")]
        [SerializeField] private UIDocument _uiDocument;

        private CircularProgressBar _progressBar1;
        private CircularProgressBar _progressBar2;
        private CircularProgressBar _progressBar3;
        private CircularProgressBar _filledProgressBar1;
        private CircularProgressBar _filledProgressBar2;
        private CircularProgressBar _filledProgressBar3;
        private CircularProgressBar _sectoredRingBar;
        private CircularProgressBar _sectoredFilledBar;
        private CircularProgressBar _discreteSectorBar;
        private VisualElement _root;
        private VisualElement _container;

        private void OnValidate()
        {
            if (_uiDocument == null)
                _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            InitializeUI();

            if (_runTestOnStart)
            {
                StartCoroutine(RunProgressBarTest());
            }
        }

        private void InitializeUI()
        {
            if (_uiDocument == null)
            {
                Debug.LogError("UIDocument is not assigned!");
                return;
            }

            _root = _uiDocument.rootVisualElement;

            // Find Ring progress bars from UXML (configured in editor)
            _progressBar1 = _root.Q<CircularProgressBar>("progress-bar-1");
            _progressBar2 = _root.Q<CircularProgressBar>("progress-bar-2");
            _progressBar3 = _root.Q<CircularProgressBar>("progress-bar-3");

            if (_progressBar1 == null || _progressBar2 == null || _progressBar3 == null)
            {
                Debug.LogError("Could not find Ring progress bars in UXML! Make sure they have correct names.");
                return;
            }

            _progressBar1.FillColor = new Color(0.7f, 0.2f, 1f, 1f);
            _progressBar2.FillColor = new Color(0.2f, 0.8f, 0.8f, 1f);
            _progressBar3.FillColor = new Color(1f, 0.4f, 0.7f, 1f);
            
            Debug.Log("Found Ring progress bars from UXML (configured in editor)");

            // Find or create container for code-generated Filled bars
            _container = _root.Q<VisualElement>("code-generated-container");
            if (_container == null)
            {
                Debug.LogError("Could not find code-generated-container in UXML!");
                return;
            }

            // Create Filled Progress Bar 1 - Purple (from code)
            var filledWrapper1 = CreateProgressBarWrapper("Filled Bar 1 (Code)", new Color(0.7f, 0.2f, 1f));
            _filledProgressBar1 = new CircularProgressBar
            {
                Progress = 0f,
                FillColor = new Color(0.7f, 0.2f, 1f, 1f),
                BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                Radius = 60f,
                Mode = FillMode.Filled
            };
            filledWrapper1.Add(_filledProgressBar1);
            _container.Add(filledWrapper1);

            // Create Filled Progress Bar 2 - Teal (from code)
            var filledWrapper2 = CreateProgressBarWrapper("Filled Bar 2 (Code)", new Color(0.2f, 0.8f, 0.8f));
            _filledProgressBar2 = new CircularProgressBar
            {
                Progress = 0f,
                FillColor = new Color(0.2f, 0.8f, 0.8f, 1f),
                BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                Radius = 50f,
                Mode = FillMode.Filled
            };
            filledWrapper2.Add(_filledProgressBar2);
            _container.Add(filledWrapper2);

            // Create Filled Progress Bar 3 - Pink (from code)
            var filledWrapper3 = CreateProgressBarWrapper("Filled Bar 3 (Code)", new Color(1f, 0.4f, 0.7f));
            _filledProgressBar3 = new CircularProgressBar
            {
                Progress = 0f,
                FillColor = new Color(1f, 0.4f, 0.7f, 1f),
                BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                Radius = 70f,
                Mode = FillMode.Filled
            };
            filledWrapper3.Add(_filledProgressBar3);
            _container.Add(filledWrapper3);

            // Create Sectored Ring Bar (from code)
            var sectoredRingWrapper = CreateProgressBarWrapper("Sectored Ring (5 sectors)", new Color(0.2f, 0.6f, 1f));
            _sectoredRingBar = new CircularProgressBar
            {
                Progress = 0f,
                FillColor = new Color(0.2f, 0.6f, 1f, 1f),
                BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                Radius = 60f,
                Thickness = 12f,
                Mode = FillMode.Ring,
                SectorCount = 5,
                SectorGap = 6f,
                DiscreteProgress = false
            };
            sectoredRingWrapper.Add(_sectoredRingBar);
            _container.Add(sectoredRingWrapper);

            // Create Sectored Filled Bar (from code)
            var sectoredFilledWrapper = CreateProgressBarWrapper("Sectored Filled (6 sectors)", new Color(1f, 0.5f, 0.2f));
            _sectoredFilledBar = new CircularProgressBar
            {
                Progress = 0f,
                FillColor = new Color(1f, 0.5f, 0.2f, 1f),
                BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                Radius = 55f,
                Mode = FillMode.Filled,
                SectorCount = 6,
                SectorGap = 8f,
                DiscreteProgress = false
            };
            sectoredFilledWrapper.Add(_sectoredFilledBar);
            _container.Add(sectoredFilledWrapper);

            // Create Discrete Sector Bar (from code)
            var discreteWrapper = CreateProgressBarWrapper("Discrete (4 sectors)", new Color(0.8f, 0.2f, 0.8f));
            _discreteSectorBar = new CircularProgressBar
            {
                Progress = 0f,
                FillColor = new Color(0.8f, 0.2f, 0.8f, 1f),
                BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f),
                Radius = 50f,
                Thickness = 10f,
                Mode = FillMode.Ring,
                SectorCount = 4,
                SectorGap = 5f,
                DiscreteProgress = true
            };
            discreteWrapper.Add(_discreteSectorBar);
            _container.Add(discreteWrapper);

            Debug.Log("=== CIRCULAR PROGRESS BAR UI INITIALIZED ===");
            Debug.Log("3 Ring bars loaded from UXML (configured in editor)");
            Debug.Log("3 Filled bars created from code");
            Debug.Log("3 Sectored bars created from code (Ring, Filled, Discrete)");
        }

        private VisualElement CreateProgressBarWrapper(string title, Color titleColor)
        {
            var wrapper = new VisualElement();
            wrapper.style.flexDirection = FlexDirection.Column;
            wrapper.style.alignItems = Align.Center;
            wrapper.style.marginLeft = 20;
            wrapper.style.marginRight = 20;
            wrapper.style.marginTop = 20;
            wrapper.style.marginBottom = 20;

            var label = new Label(title);
            label.style.fontSize = 18;
            label.style.color = titleColor;
            label.style.marginBottom = 10;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            wrapper.Add(label);

            return wrapper;
        }

        private IEnumerator RunProgressBarTest()
        {
            Debug.Log("=== CIRCULAR PROGRESS BAR TEST STARTED ===");
            yield return new WaitForSeconds(_delayBetweenActions);

            // ========== SECTORED BARS TESTS (First) ==========

            // Test 1: Sectored bars - Fill to 50%
            Debug.Log("TEST 1: Sectored bars - Fill to 50%");
            SetAllSectoredBars(0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 2: Sectored bars - Fill to 75%
            Debug.Log("TEST 2: Sectored bars - Fill to 75%");
            SetAllSectoredBars(0.75f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 3: Sectored bars - Animated fill to 100%
            Debug.Log("TEST 3: Sectored bars - Animated fill to 100%");
            _sectoredRingBar.SetProgressAnimated(1.0f, 2.0f);
            _sectoredFilledBar.SetProgressAnimated(1.0f, 2.0f);
            _discreteSectorBar.SetProgressAnimated(1.0f, 2.0f);
            yield return new WaitForSeconds(2.5f);

            // Test 4: Sectored bars - Different progress values
            Debug.Log("TEST 4: Sectored bars - Different progress values");
            _sectoredRingBar.Progress = 0.4f;
            _sectoredFilledBar.Progress = 0.6f;
            _discreteSectorBar.Progress = 0.7f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 5: Toggle discrete mode on sectored ring bar
            Debug.Log("TEST 5: Toggle discrete mode on sectored ring bar");
            _sectoredRingBar.DiscreteProgress = true;
            _sectoredRingBar.Progress = 0.5f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 6: Change sector count dynamically
            Debug.Log("TEST 6: Change sector count to 8");
            _sectoredRingBar.SectorCount = 8;
            _sectoredRingBar.Progress = 0.6f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 7: Change gap size
            Debug.Log("TEST 7: Change gap size to 12");
            _sectoredFilledBar.SectorGap = 12f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 8: Reset sectored bars
            Debug.Log("TEST 8: Reset sectored bars");
            _sectoredRingBar.DiscreteProgress = false;
            _sectoredRingBar.SectorCount = 5;
            _sectoredRingBar.SectorGap = 6f;
            _sectoredFilledBar.SectorGap = 8f;
            SetAllSectoredBars(0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // ========== RING BARS TESTS ==========

            // Test 9: Fill all bars to 25%
            Debug.Log("TEST 9: Fill all bars to 25%");
            SetAllProgressBars(0.25f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 2: Fill all bars to 50%
            Debug.Log("TEST 2: Fill all bars to 50%");
            SetAllProgressBars(0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 3: Fill all bars to 75%
            Debug.Log("TEST 3: Fill all bars to 75%");
            SetAllProgressBars(0.75f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 4: Fill all bars to 100%
            Debug.Log("TEST 4: Fill all bars to 100%");
            SetAllProgressBars(1.0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 5: Reset all bars to 0%
            Debug.Log("TEST 5: Reset all bars to 0%");
            SetAllProgressBars(0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 6: Animated fill - Bar 1 to 80%
            Debug.Log("TEST 6: Animated fill - Bar 1 to 80% (2 seconds)");
            _progressBar1.SetProgressAnimated(0.8f, 2.0f);
            yield return new WaitForSeconds(2.5f);

            // Test 7: Animated fill - Bar 2 to 60%
            Debug.Log("TEST 7: Animated fill - Bar 2 to 60% (1.5 seconds)");
            _progressBar2.SetProgressAnimated(0.6f, 1.5f);
            yield return new WaitForSeconds(2.0f);

            // Test 8: Animated fill - Bar 3 to 90%
            Debug.Log("TEST 8: Animated fill - Bar 3 to 90% (2.5 seconds)");
            _progressBar3.SetProgressAnimated(0.9f, 2.5f);
            yield return new WaitForSeconds(3.0f);

            // Test 9: Change color of Bar 1
            Debug.Log("TEST 9: Change Bar 1 color to Purple");
            _progressBar1.FillColor = new Color(0.7f, 0.2f, 1f, 1f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 10: Change color of Bar 2
            Debug.Log("TEST 10: Change Bar 2 color to Yellow");
            _progressBar2.FillColor = new Color(1f, 0.9f, 0.2f, 1f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 11: Change color of Bar 3
            Debug.Log("TEST 11: Change Bar 3 color to Magenta");
            _progressBar3.FillColor = new Color(1f, 0.2f, 0.7f, 1f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 12: Change thickness of Bar 1
            Debug.Log("TEST 12: Change Bar 1 thickness to 20");
            _progressBar1.Thickness = 20f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 13: Change thickness of Bar 2
            Debug.Log("TEST 13: Change Bar 2 thickness to 3");
            _progressBar2.Thickness = 3f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 14: Change radius of Bar 3
            Debug.Log("TEST 14: Change Bar 3 radius to 40");
            _progressBar3.Radius = 40f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 15: Reset all to original state with animation
            Debug.Log("TEST 15: Reset all to original state");
            _progressBar1.FillColor = new Color(0.2f, 0.8f, 0.2f, 1f);
            _progressBar1.Thickness = 12f;
            _progressBar2.FillColor = new Color(0.2f, 0.5f, 1f, 1f);
            _progressBar2.Thickness = 8f;
            _progressBar3.FillColor = new Color(1f, 0.3f, 0.1f, 1f);
            _progressBar3.Radius = 70f;
            SetAllProgressBars(0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 16: Simultaneous animated fills with different speeds
            Debug.Log("TEST 16: Simultaneous animated fills with different speeds");
            _progressBar1.SetProgressAnimated(1.0f, 3.0f);
            _progressBar2.SetProgressAnimated(1.0f, 2.0f);
            _progressBar3.SetProgressAnimated(1.0f, 4.0f);
            yield return new WaitForSeconds(4.5f);

            // Test 17: Rapid progress changes (simulating real-time updates)
            Debug.Log("TEST 17: Rapid progress changes (0.1s intervals)");
            for (float p = 0f; p <= 1f; p += 0.1f)
            {
                SetAllProgressBars(p);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 18: Reverse rapid changes
            Debug.Log("TEST 18: Reverse rapid changes (0.1s intervals)");
            for (float p = 1f; p >= 0f; p -= 0.1f)
            {
                SetAllProgressBars(p);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 19: Different progress values for each bar
            Debug.Log("TEST 19: Set different progress values");
            _progressBar1.Progress = 0.3f;
            _progressBar2.Progress = 0.6f;
            _progressBar3.Progress = 0.9f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 20: Change background colors
            Debug.Log("TEST 20: Change background ring colors");
            _progressBar1.BackgroundRingColor = new Color(0.5f, 0.2f, 0.2f, 0.5f);
            _progressBar2.BackgroundRingColor = new Color(0.2f, 0.2f, 0.5f, 0.5f);
            _progressBar3.BackgroundRingColor = new Color(0.5f, 0.5f, 0.2f, 0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 21: Reset background colors
            Debug.Log("TEST 21: Reset background colors");
            _progressBar1.BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f);
            _progressBar2.BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f);
            _progressBar3.BackgroundRingColor = new Color(0.2f, 0.2f, 0.2f, 0.3f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 22: Test filled bars - Fill to 50%
            Debug.Log("TEST 22: Test filled bars - Fill to 50%");
            SetAllFilledBars(0.5f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 23: Test filled bars - Fill to 75%
            Debug.Log("TEST 23: Test filled bars - Fill to 75%");
            SetAllFilledBars(0.75f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 24: Test filled bars - Animated fill to 100%
            Debug.Log("TEST 24: Test filled bars - Animated fill to 100%");
            _filledProgressBar1.SetProgressAnimated(1.0f, 2.0f);
            _filledProgressBar2.SetProgressAnimated(1.0f, 1.5f);
            _filledProgressBar3.SetProgressAnimated(1.0f, 2.5f);
            yield return new WaitForSeconds(3.0f);

            // Test 25: Test filled bars - Different progress values
            Debug.Log("TEST 25: Test filled bars - Different progress values");
            _filledProgressBar1.Progress = 0.3f;
            _filledProgressBar2.Progress = 0.6f;
            _filledProgressBar3.Progress = 0.9f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 26: Test mode switching - Ring to Filled
            Debug.Log("TEST 26: Switch Bar 1 from Ring to Filled mode");
            _progressBar1.Mode = FillMode.Filled;
            _progressBar1.Progress = 0.7f;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 27: Switch Bar 1 back to Ring mode
            Debug.Log("TEST 27: Switch Bar 1 back to Ring mode");
            _progressBar1.Mode = FillMode.Ring;
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 28: Reset filled bars
            Debug.Log("TEST 28: Reset filled bars to 0%");
            SetAllFilledBars(0f);
            yield return new WaitForSeconds(_delayBetweenActions);

            // Test 29: Final showcase - all ring bars to 100%
            Debug.Log("TEST 29: Final showcase - ring bars animated fill to 100%");
            _progressBar1.SetProgressAnimated(1.0f, 2.0f);
            _progressBar2.SetProgressAnimated(1.0f, 2.0f);
            _progressBar3.SetProgressAnimated(1.0f, 2.0f);
            yield return new WaitForSeconds(2.5f);

            // Test 38: Final showcase - all filled bars to 100%
            Debug.Log("TEST 38: Final showcase - filled bars animated fill to 100%");
            _filledProgressBar1.SetProgressAnimated(1.0f, 2.0f);
            _filledProgressBar2.SetProgressAnimated(1.0f, 2.0f);
            _filledProgressBar3.SetProgressAnimated(1.0f, 2.0f);
            yield return new WaitForSeconds(2.5f);

            Debug.Log("=== CIRCULAR PROGRESS BAR TEST COMPLETED ===");
        }

        private void SetAllProgressBars(float progress)
        {
            if (_progressBar1 != null) _progressBar1.Progress = progress;
            if (_progressBar2 != null) _progressBar2.Progress = progress;
            if (_progressBar3 != null) _progressBar3.Progress = progress;
        }

        private void SetAllFilledBars(float progress)
        {
            if (_filledProgressBar1 != null) _filledProgressBar1.Progress = progress;
            if (_filledProgressBar2 != null) _filledProgressBar2.Progress = progress;
            if (_filledProgressBar3 != null) _filledProgressBar3.Progress = progress;
        }

        private void SetAllSectoredBars(float progress)
        {
            if (_sectoredRingBar != null) _sectoredRingBar.Progress = progress;
            if (_sectoredFilledBar != null) _sectoredFilledBar.Progress = progress;
            if (_discreteSectorBar != null) _discreteSectorBar.Progress = progress;
        }

        [ContextMenu("Run Test Now")]
        public void RunTestNow()
        {
            if (_progressBar1 == null)
            {
                InitializeUI();
            }
            StartCoroutine(RunProgressBarTest());
        }

        [ContextMenu("Set All to 25%")]
        public void SetAllTo25()
        {
            SetAllProgressBars(0.25f);
        }

        [ContextMenu("Set All to 50%")]
        public void SetAllTo50()
        {
            SetAllProgressBars(0.5f);
        }

        [ContextMenu("Set All to 75%")]
        public void SetAllTo75()
        {
            SetAllProgressBars(0.75f);
        }

        [ContextMenu("Set All to 100%")]
        public void SetAllTo100()
        {
            SetAllProgressBars(1.0f);
        }

        [ContextMenu("Reset All to 0%")]
        public void ResetAll()
        {
            SetAllProgressBars(0f);
        }

        [ContextMenu("Animate Bar 1 to 100%")]
        public void AnimateBar1()
        {
            if (_progressBar1 != null)
                _progressBar1.SetProgressAnimated(1.0f, 2.0f);
        }

        [ContextMenu("Change Bar 1 Color (Random)")]
        public void ChangeBar1Color()
        {
            if (_progressBar1 != null)
                _progressBar1.FillColor = new Color(Random.value, Random.value, Random.value, 1f);
        }

        [ContextMenu("Change Bar 1 Thickness (Random 5-20)")]
        public void ChangeBar1Thickness()
        {
            if (_progressBar1 != null)
                _progressBar1.Thickness = Random.Range(5f, 20f);
        }

        [ContextMenu("Continuous Update Test")]
        public void ContinuousUpdateTest()
        {
            StartCoroutine(ContinuousUpdate());
        }

        [ContextMenu("Set All Filled Bars to 50%")]
        public void SetAllFilledTo50()
        {
            SetAllFilledBars(0.5f);
        }

        [ContextMenu("Set All Filled Bars to 100%")]
        public void SetAllFilledTo100()
        {
            SetAllFilledBars(1.0f);
        }

        [ContextMenu("Reset All Filled Bars to 0%")]
        public void ResetAllFilled()
        {
            SetAllFilledBars(0f);
        }

        [ContextMenu("Toggle Bar 1 Mode (Ring/Filled)")]
        public void ToggleBar1Mode()
        {
            if (_progressBar1 != null)
            {
                _progressBar1.Mode = _progressBar1.Mode == FillMode.Ring ? FillMode.Filled : FillMode.Ring;
                Debug.Log($"Bar 1 mode changed to: {_progressBar1.Mode}");
            }
        }

        [ContextMenu("Animate Filled Bar 1 to 100%")]
        public void AnimateFilledBar1()
        {
            if (_filledProgressBar1 != null)
                _filledProgressBar1.SetProgressAnimated(1.0f, 2.0f);
        }

        private IEnumerator ContinuousUpdate()
        {
            Debug.Log("Starting continuous update test (10 seconds)");
            float elapsed = 0f;
            while (elapsed < 10f)
            {
                float progress = Mathf.PingPong(elapsed * 0.5f, 1f);
                SetAllProgressBars(progress);
                SetAllFilledBars(progress);
                SetAllSectoredBars(progress);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Continuous update test completed");
        }

        [ContextMenu("Set All Sectored Bars to 50%")]
        public void SetAllSectoredTo50()
        {
            SetAllSectoredBars(0.5f);
        }

        [ContextMenu("Set All Sectored Bars to 100%")]
        public void SetAllSectoredTo100()
        {
            SetAllSectoredBars(1.0f);
        }

        [ContextMenu("Reset All Sectored Bars")]
        public void ResetAllSectored()
        {
            SetAllSectoredBars(0f);
        }

        [ContextMenu("Toggle Discrete Mode on Sectored Ring")]
        public void ToggleSectoredRingDiscrete()
        {
            if (_sectoredRingBar != null)
            {
                _sectoredRingBar.DiscreteProgress = !_sectoredRingBar.DiscreteProgress;
                Debug.Log($"Sectored Ring discrete mode: {_sectoredRingBar.DiscreteProgress}");
            }
        }

        [ContextMenu("Change Sectored Ring Sector Count (Random 3-10)")]
        public void ChangeSectoredRingSectorCount()
        {
            if (_sectoredRingBar != null)
            {
                _sectoredRingBar.SectorCount = Random.Range(3, 11);
                Debug.Log($"Sectored Ring sector count: {_sectoredRingBar.SectorCount}");
            }
        }
    }
}
