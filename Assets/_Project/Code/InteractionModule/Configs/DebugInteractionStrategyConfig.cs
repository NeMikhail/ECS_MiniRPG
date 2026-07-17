using UnityEngine;

namespace ECSMiniRPG.InteractionModule.Configs
{
    [CreateAssetMenu(fileName = "DebugInteractionStrategyConfig", menuName = "ECSMiniRPG/Interaction/Debug Strategy")]
    public sealed class DebugInteractionStrategyConfig : InteractionStrategyConfig
    {
        [SerializeField] private string _message = "Debug interaction completed.";

        public override bool CanInteract(InteractionContext context)
        {
            return true;
        }

        public override void Interact(InteractionContext context)
        {
            Debug.Log(_message);
        }
    }
}
