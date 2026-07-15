using ECSMiniRPG.PlayerModule.Configs;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG
{
    [CreateAssetMenu(fileName = ConfigAssetNames._gameConfigsFileName, menuName = ConfigAssetNames._gameConfigsMenuName)]
    public sealed class GameConfigs : ScriptableObject, IResource
    {

        [SerializeField] private PlayerConfig _playerConfig;

        public PlayerConfig PlayerConfig => _playerConfig;
    }
}
