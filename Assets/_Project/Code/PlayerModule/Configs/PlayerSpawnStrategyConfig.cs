using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Configs
{
    public abstract class PlayerSpawnStrategyConfig : ScriptableObject
    {
        public virtual bool RequiresLocationData => false;

        public abstract bool TryResolve(PlayerSpawnResolveContext resolveContext, out PlayerSpawnPose spawnPose);
    }
}
