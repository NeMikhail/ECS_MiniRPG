using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.PlayerModule.Configs
{
    public sealed class PlayerSpawnRuntimeData : IResource
    {
        public PlayerSpawnStrategyConfig CurrentStrategy { get; private set; }

        public PlayerSpawnRuntimeData(PlayerSpawnStrategyConfig currentStrategy)
        {
            CurrentStrategy = currentStrategy;
        }

        public void SetCurrentStrategy(PlayerSpawnStrategyConfig currentStrategy)
        {
            CurrentStrategy = currentStrategy;
        }
    }
}
