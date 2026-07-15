using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Configs
{
    [CreateAssetMenu(fileName = ConfigAssetNames._presetPlayerSpawnStrategyConfigFileName, menuName = ConfigAssetNames._presetPlayerSpawnStrategyConfigMenuName)]
    public sealed class PresetPlayerSpawnStrategyConfig : PlayerSpawnStrategyConfig
    {
        [SerializeField] private Vector3 _spawnPosition = Vector3.zero;
        [SerializeField] private Vector3 _spawnEulerAngles = Vector3.zero;

        public override bool TryResolve(PlayerSpawnResolveContext resolveContext, out PlayerSpawnPose spawnPose)
        {
            spawnPose = new PlayerSpawnPose(_spawnPosition, Quaternion.Euler(_spawnEulerAngles));
            return true;
        }
    }
}
