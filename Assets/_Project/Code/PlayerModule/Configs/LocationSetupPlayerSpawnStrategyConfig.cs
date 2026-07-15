using ECSMiniRPG.LocationModule;
using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Configs
{
    [CreateAssetMenu(fileName = ConfigAssetNames._locationSetupPlayerSpawnStrategyConfigFileName, menuName = ConfigAssetNames._locationSetupPlayerSpawnStrategyConfigMenuName)]
    public sealed class LocationSetupPlayerSpawnStrategyConfig : PlayerSpawnStrategyConfig
    {
        [SerializeField] private string _spawnPointId = LocationSpawnPointIds._playerStart;

        public override bool RequiresLocationData => true;

        public override bool TryResolve(PlayerSpawnResolveContext resolveContext, out PlayerSpawnPose spawnPose)
        {
            var hasSpawnPose = false;
            spawnPose = PlayerSpawnPose.Identity;

            if (resolveContext.LocationRuntimeData == null)
            {
                Debug.LogError("Location runtime data is required for location setup player spawn strategy.");
            }

            if (resolveContext.LocationRuntimeData != null)
            {
                hasSpawnPose = resolveContext.LocationRuntimeData.TryGetSpawnPose(_spawnPointId, out spawnPose);
            }

            return hasSpawnPose;
        }
    }
}
