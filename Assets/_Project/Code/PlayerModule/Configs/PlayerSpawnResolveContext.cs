using ECSMiniRPG.LocationModule;

namespace ECSMiniRPG.PlayerModule.Configs
{
    public readonly struct PlayerSpawnResolveContext
    {
        public readonly GameConfigs GameConfigs;
        public readonly LocationRuntimeData LocationRuntimeData;

        public PlayerSpawnResolveContext(GameConfigs gameConfigs, LocationRuntimeData locationRuntimeData)
        {
            GameConfigs = gameConfigs;
            LocationRuntimeData = locationRuntimeData;
        }
    }
}
