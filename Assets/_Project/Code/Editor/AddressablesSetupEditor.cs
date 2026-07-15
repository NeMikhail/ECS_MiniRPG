#if UNITY_EDITOR
using ECSMiniRPG.ContentManagement;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace ECSMiniRPG.Editor
{
    public static class AddressablesSetupEditor
    {
        private const string _playerPrefabPath = "Assets/_Project/Prefabs/Player/Player.prefab";
        private const string _menuPath = "Tools/ECS MiniRPG/Setup Addressables";

        [MenuItem(_menuPath)]
        public static void SetupAddressables()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.DefaultGroup;
            var guid = AssetDatabase.AssetPathToGUID(_playerPrefabPath);
            var entry = settings.CreateOrMoveEntry(guid, group);

            entry.address = ContentKeys.PlayerPrefab;
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
