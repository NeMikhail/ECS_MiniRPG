using System.Collections.Generic;
using System.Linq;
using MAEngine.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MAEngine.PrefabVariantBuilder
{
    internal class PrefabVariantBuilderWindow : EditorWindow
    {
        private static readonly string WindowTitle = "Prefab Variant Builder";
        private static readonly string DefaultOutputFolder = "Assets/_Project/Generated/PrefabVariants";
        private static readonly string ReplaceModeLabel = "Replace Source Prefabs";
        private static readonly string RootSourceLabel = "Root Prefab";

        private bool _useFolderMode = true;
        private DefaultAsset _sourceFolder;
        private List<GameObject> _sourcePrefabs = new List<GameObject>();
        private string _rootPrefabName = "RootPrefab";
        private string _outputFolder = DefaultOutputFolder;
        private float _majorityThreshold = 0.5f;
        private bool _isReplaceMode = false;
        private GameObject _rootSourcePrefab;

        private ReorderableList _prefabList;
        private Vector2 _listScroll;
        private string _lastLog = string.Empty;

        [MenuItem("Tools/MAEngine/Prefab Variant Builder")]
        private static void Open()
        {
            GetWindow<PrefabVariantBuilderWindow>(WindowTitle);
        }

        private void OnEnable()
        {
            _prefabList = new ReorderableList(_sourcePrefabs, typeof(GameObject), true, true, true, true);
            _prefabList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Source Prefabs");
            _prefabList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                _sourcePrefabs[index] = (GameObject)EditorGUI.ObjectField(
                    rect, _sourcePrefabs[index], typeof(GameObject), false);
            };
            _prefabList.onAddCallback = list => _sourcePrefabs.Add(null);
            _prefabList.onRemoveCallback = list =>
            {
                if (list.index >= 0 && list.index < _sourcePrefabs.Count)
                    _sourcePrefabs.RemoveAt(list.index);
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);
            _useFolderMode = EditorGUILayout.Toggle("Use Folder Mode", _useFolderMode);

            EditorGUILayout.Space();

            if (_useFolderMode)
            {
                _sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField(
                    "Source Folder", _sourceFolder, typeof(DefaultAsset), false);
            }
            else
            {
                _listScroll = EditorGUILayout.BeginScrollView(_listScroll, GUILayout.MaxHeight(200));
                _prefabList.DoLayoutList();
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            if (!_isReplaceMode || _rootSourcePrefab == null)
                _rootPrefabName = EditorGUILayout.TextField("Root Prefab Name", _rootPrefabName);
            _majorityThreshold = EditorGUILayout.Slider("Majority Threshold", _majorityThreshold, 0.1f, 1.0f);
            _isReplaceMode = EditorGUILayout.Toggle(ReplaceModeLabel, _isReplaceMode);
            if (_isReplaceMode)
                _rootSourcePrefab = (GameObject)EditorGUILayout.ObjectField(RootSourceLabel, _rootSourcePrefab, typeof(GameObject), false);

            if (!_isReplaceMode)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
                _outputFolder = MAEditorGUI.FolderField("Output Folder", _outputFolder);
            }

            EditorGUILayout.Space();

            var canBuild = CanBuild();
            EditorGUI.BeginDisabledGroup(!canBuild);
            if (GUILayout.Button("Build Variants", GUILayout.Height(30)))
                RunBuild();
            EditorGUI.EndDisabledGroup();

            if (!string.IsNullOrEmpty(_lastLog))
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(_lastLog, MessageType.Info);
            }
        }

        private bool CanBuild()
        {
            if (_useFolderMode && _sourceFolder == null) return false;
            if (!_useFolderMode && !_sourcePrefabs.Any(p => p != null)) return false;
            if (_isReplaceMode && _rootSourcePrefab != null) return true;
            if (_isReplaceMode) return !string.IsNullOrEmpty(_rootPrefabName);
            return !string.IsNullOrEmpty(_rootPrefabName) && !string.IsNullOrEmpty(_outputFolder);
        }

        private void RunBuild()
        {
            var paths = CollectSourcePaths();
            if (paths.Count < 2)
            {
                _lastLog = "Need at least 2 source prefabs to build variants.";
                return;
            }

            var analysis = PrefabAnalyzer.Analyze(paths, _majorityThreshold);

            var hasExplicitRoot = _isReplaceMode && _rootSourcePrefab != null;
            var rootSourcePath = hasExplicitRoot ? AssetDatabase.GetAssetPath(_rootSourcePrefab) : string.Empty;
            var rootName = hasExplicitRoot ? _rootSourcePrefab.name : _rootPrefabName;
            var rootFolder = hasExplicitRoot ? rootSourcePath.Substring(0, rootSourcePath.LastIndexOf('/'))
                : _isReplaceMode ? ResolveSourceFolder(paths)
                : _outputFolder;
            var rootAssetPath = hasExplicitRoot
                ? rootSourcePath
                : rootFolder + "/" + _rootPrefabName + ".prefab";

            // Build root outside batch editing — it must be imported before variants can reference it.
            // Если указан явный root-префаб, используем его как есть и не пересохраняем,
            // чтобы не конвертировать prefab variant в обычный префаб.
            string rootPath;
            if (hasExplicitRoot)
            {
                rootPath = rootSourcePath;
                AssetDatabase.ImportAsset(rootPath, ImportAssetOptions.ForceUpdate);
            }
            else
            {
                rootPath = RootPrefabBuilder.Build(analysis, rootName, rootAssetPath);
                AssetDatabase.ImportAsset(rootPath, ImportAssetOptions.ForceUpdate);
            }

            // Build all variants in a single batch import pass
            AssetDatabase.StartAssetEditing();
            List<string> variantPaths;
            try
            {
                variantPaths = VariantPrefabBuilder.BuildAll(analysis, rootPath, _outputFolder, _isReplaceMode, rootSourcePath);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }

            var variantLabel = _isReplaceMode ? "Replaced" : "Variants created";
            _lastLog = $"Root: {rootName}.prefab\n"
                     + $"{variantLabel}: {variantPaths.Count}\n"
                     + $"Shared entries: {analysis.SharedEntries.Count}\n"
                     + $"Output: {rootFolder}";
        }

        private string ResolveSourceFolder(List<string> paths)
        {
            if (_useFolderMode)
                return AssetDatabase.GetAssetPath(_sourceFolder);
            var firstPath = paths.FirstOrDefault();
            if (string.IsNullOrEmpty(firstPath)) return string.Empty;
            var lastSlash = firstPath.LastIndexOf('/');
            return lastSlash >= 0 ? firstPath.Substring(0, lastSlash) : string.Empty;
        }

        private List<string> CollectSourcePaths()
        {
            if (_useFolderMode)
            {
                var folderPath = AssetDatabase.GetAssetPath(_sourceFolder);
                if (string.IsNullOrEmpty(folderPath)) return new List<string>();

                var guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
                return guids.Select(AssetDatabase.GUIDToAssetPath).ToList();
            }

            return _sourcePrefabs
                .Where(p => p != null)
                .Select(AssetDatabase.GetAssetPath)
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .ToList();
        }

    }
}
