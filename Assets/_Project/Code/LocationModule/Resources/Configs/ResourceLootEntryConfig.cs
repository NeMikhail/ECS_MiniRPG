using System;
using ECSMiniRPG.InventoryModule.Items;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Configs
{
    [Serializable]
    public sealed class ResourceLootEntryConfig
    {
        [SerializeField] private ItemPreset _item;
        [SerializeField] private int _minCount = 1;
        [SerializeField] private int _maxCount = 1;

        public ItemPreset Item => _item;
        public int MinCount => Mathf.Max(0, _minCount);
        public int MaxCount => Mathf.Max(MinCount, _maxCount);
    }
}