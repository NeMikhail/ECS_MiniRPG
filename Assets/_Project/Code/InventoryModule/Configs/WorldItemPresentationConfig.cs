using ECSMiniRPG.ContentManagement;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Configs
{
    [CreateAssetMenu(fileName = "WorldItemPresentationConfig", menuName = "ECSMiniRPG/Configs/World Item Presentation")]
    public sealed class WorldItemPresentationConfig : ScriptableObject
    {
        [SerializeField] private string _lootPrefabKey = ContentKeys.DefaultLootObjectPrefab;

        public string LootPrefabKey => _lootPrefabKey;
        public bool HasLootPrefab => !string.IsNullOrWhiteSpace(_lootPrefabKey);
    }
}