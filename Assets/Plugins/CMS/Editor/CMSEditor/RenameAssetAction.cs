using System.IO;
using UnityEditor;

namespace CMS.Editor
{
    public class RenameAssetAction : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            AssetDatabase.RenameAsset(pathName, Path.GetFileNameWithoutExtension(pathName));
            AssetDatabase.Refresh();
        }
    }
}