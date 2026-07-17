using ECSMiniRPG.InteractionModule.Configs;
using UnityEngine;

namespace ECSMiniRPG.InteractionModule.Views
{
    public sealed class InteractableView : MonoBehaviour
    {
        [SerializeField] private InteractionStrategyConfig _strategy;
        [SerializeField] private float _interactionRadius = 2f;

        public InteractionStrategyConfig Strategy => _strategy;
        public float InteractionRadius => Mathf.Max(0f, _interactionRadius);
    }
}
