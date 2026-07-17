using System.Collections.Generic;
using ECSMiniRPG.InventoryModule.Configs;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Items
{
    public abstract class ItemPreset : ScriptableObject, IItem
    {
        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [SerializeField] private Sprite _icon;
        [SerializeField] private WorldItemPresentationConfig _worldPresentation;
        [SerializeField] private int _maxStack = 1;
        [SerializeField] private List<ItemStatModifierConfig> _statModifiers = new List<ItemStatModifierConfig>();

        public string Id => _id;
        public string Title => _title;
        public Sprite Icon => _icon;
        public WorldItemPresentationConfig WorldPresentation => _worldPresentation;
        public bool HasWorldLootPrefab => _worldPresentation != null && _worldPresentation.HasLootPrefab;
        public int MaxStack => Mathf.Max(1, _maxStack);
        public IReadOnlyList<ItemStatModifierConfig> StatModifiers => _statModifiers;
    }
}