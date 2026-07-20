using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    public abstract class MobSpawnBehaviourConfig : ScriptableObject
    {
        public abstract void Execute(MobSpawnContext context);
    }
}
