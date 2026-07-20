using ECSMiniRPG.InventoryModule.Configs;
using ECSMiniRPG.LoggerModule.Configs;
using ECSMiniRPG.PlayerModule.Configs;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG
{
    [CreateAssetMenu(fileName = ConfigAssetNames._gameConfigsFileName, menuName = ConfigAssetNames._gameConfigsMenuName)]
    public sealed class GameConfigs : ScriptableObject, IResource
    {

        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private InventoryConfig _inventoryConfig;
        [SerializeField] private LoggerModuleConfig _loggerModuleConfig;

        public PlayerConfig PlayerConfig => _playerConfig;
        public InventoryConfig InventoryConfig => _inventoryConfig;
        public LoggerModuleConfig LoggerModuleConfig => _loggerModuleConfig;
    }
}

