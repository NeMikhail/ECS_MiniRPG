using System;
using System.Collections.Generic;
using ECSMiniRPG.InventoryModule.Items;
using UnityEngine;

namespace ECSMiniRPG.InventoryModule.Configs
{
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "ECSMiniRPG/Configs/Inventory Config")]
    public sealed class InventoryConfig : ScriptableObject
    {
        [SerializeField] private int _columns = 8;
        [SerializeField] private int _rows = 4;
        [SerializeField] private List<StartInventoryItemConfig> _startItems = new List<StartInventoryItemConfig>();

        public int Columns => Mathf.Max(1, _columns);
        public int Rows => Mathf.Max(1, _rows);
        public int SlotsCount => Columns * Rows;
        public IReadOnlyList<StartInventoryItemConfig> StartItems => _startItems;

        public ItemPreset GetItemPreset(string id)
        {
            ItemPreset preset = null;

            for (var i = 0; i < _startItems.Count; i++)
            {
                var startPreset = _startItems[i].Preset;

                if (startPreset != null && startPreset.Id == id)
                {
                    preset = startPreset;
                }
            }

            return preset;
        }
    }

    [Serializable]
    public sealed class StartInventoryItemConfig
    {
        [SerializeField] private ItemPreset _preset;
        [SerializeField] private int _count = 1;

        public ItemPreset Preset => _preset;
        public int Count => Mathf.Max(1, _count);
    }
}
