using UnityEngine;

namespace ECSMiniRPG.InteractionModule.Configs
{
    public abstract class InteractionStrategyConfig : ScriptableObject
    {
        public abstract bool CanInteract(InteractionContext context);

        public abstract void Interact(InteractionContext context);
    }
}
