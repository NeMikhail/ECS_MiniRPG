using src.Editor.CMSEditor.Utils;
using UnityEditor;

namespace CMS.Editor
{
    public static class CMSMenuItems
    {
        [MenuItem("CMS/Reload")]
        public static void CMSReload()
        {
            CMSHelpers.ReloadCMS();
        }
    }
}