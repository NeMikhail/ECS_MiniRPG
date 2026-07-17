using System.Collections.Generic;
using ECSMiniRPG.GameplayModule.LongActions;
using ECSMiniRPG.InventoryModule.Tooling;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Configs
{
    [CreateAssetMenu(fileName = "ResourceNodeConfig", menuName = "ECSMiniRPG/Location/Resource Node")]
    public sealed class ResourceNodeConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _prefabKey;
        [SerializeField] private ToolTypeConfig _requiredTool;
        [SerializeField] private LongActionCancelPolicyConfig _cancelPolicy;
        [SerializeField] private float _gatherDuration = 1f;
        [SerializeField] private float _gatherRange = 2f;
        [SerializeField] private List<ResourceLootEntryConfig> _loot = new List<ResourceLootEntryConfig>();

        public string Id => _id;
        public string PrefabKey => _prefabKey;
        public bool HasPrefab => !string.IsNullOrWhiteSpace(_prefabKey);
        public ToolTypeConfig RequiredTool => _requiredTool;
        public LongActionCancelPolicyConfig CancelPolicy => _cancelPolicy;
        public float GatherDuration => Mathf.Max(0f, _gatherDuration);
        public float GatherRange => Mathf.Max(0f, _gatherRange);
        public IReadOnlyList<ResourceLootEntryConfig> Loot => _loot;
    }
}