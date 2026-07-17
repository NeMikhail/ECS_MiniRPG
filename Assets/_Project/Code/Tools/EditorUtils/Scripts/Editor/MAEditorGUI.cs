using UnityEditor;
using UnityEngine;

namespace MAEngine.Editor
{
    public static class MAEditorGUI
    {
        /// <summary>
        /// Draws a folder path row: label + text field + "..." button.
        /// Supports drag-and-drop of a folder from the Project window onto the entire row.
        /// Returns the updated path in "Assets/..." format.
        /// </summary>
        public static string FolderField(string label, string path)
        {
            EditorGUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(label, path);
            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                var start = string.IsNullOrEmpty(path) ? Application.dataPath : AbsoluteFromAssetPath(path);
                var selected = EditorUtility.OpenFolderPanel("Select " + label, start, string.Empty);
                if (!string.IsNullOrEmpty(selected))
                    path = AbsoluteToAssetPath(selected);
            }
            EditorGUILayout.EndHorizontal();

            var rowRect = GUILayoutUtility.GetLastRect();
            HandleFolderDrop(rowRect, ref path);

            return path;
        }

        private static void HandleFolderDrop(Rect rect, ref string path)
        {
            var evt = Event.current;

            if (evt.type == EventType.Repaint
                && rect.Contains(evt.mousePosition)
                && DragAndDrop.visualMode == DragAndDropVisualMode.Copy)
            {
                DrawDropHighlight(rect);
            }

            if (!rect.Contains(evt.mousePosition))
                return;

            if (evt.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = GetFolderDragPath() != null
                    ? DragAndDropVisualMode.Copy
                    : DragAndDropVisualMode.Rejected;
                evt.Use();
            }
            else if (evt.type == EventType.DragPerform)
            {
                var folderPath = GetFolderDragPath();
                if (folderPath != null)
                {
                    DragAndDrop.AcceptDrag();
                    path = folderPath;
                    GUI.FocusControl(null);
                    evt.Use();
                }
            }
        }

        private static void DrawDropHighlight(Rect rect)
        {
            var fill = new Color(0.3f, 0.7f, 1f, 0.15f);
            var border = new Color(0.3f, 0.7f, 1f, 0.8f);
            EditorGUI.DrawRect(rect, fill);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1f), border);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), border);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1f, rect.height), border);
            EditorGUI.DrawRect(new Rect(rect.xMax - 1f, rect.y, 1f, rect.height), border);
        }

        private static string GetFolderDragPath()
        {
            if (DragAndDrop.objectReferences.Length != 1)
                return null;

            var obj = DragAndDrop.objectReferences[0];
            if (obj is not DefaultAsset)
                return null;

            var assetPath = AssetDatabase.GetAssetPath(obj);
            if (!AssetDatabase.IsValidFolder(assetPath))
                return null;

            return assetPath;
        }

        private static string AbsoluteToAssetPath(string absolutePath)
        {
            var dataPath = Application.dataPath.Replace('\\', '/');
            absolutePath = absolutePath.Replace('\\', '/');
            return absolutePath.StartsWith(dataPath)
                ? "Assets" + absolutePath.Substring(dataPath.Length)
                : absolutePath;
        }

        private static string AbsoluteFromAssetPath(string assetPath)
        {
            if (System.IO.Path.IsPathRooted(assetPath))
                return assetPath;
            return System.IO.Path.GetFullPath(
                System.IO.Path.Combine(Application.dataPath, "..", assetPath));
        }
    }
}
