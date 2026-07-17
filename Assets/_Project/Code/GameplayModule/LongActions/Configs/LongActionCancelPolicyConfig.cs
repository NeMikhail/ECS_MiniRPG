using UnityEngine;

namespace ECSMiniRPG.GameplayModule.LongActions
{
    [CreateAssetMenu(fileName = "LongActionCancelPolicyConfig", menuName = "ECSMiniRPG/Gameplay/Long Action Cancel Policy")]
    public sealed class LongActionCancelPolicyConfig : ScriptableObject
    {
        [SerializeField] private bool _shouldCancelOnMove = true;
        [SerializeField] private bool _shouldCancelOnDamage = true;
        [SerializeField] private bool _shouldCancelOnAction = true;
        [SerializeField] private bool _shouldCancelOnTargetOutOfRange = true;

        public bool ShouldCancelOnMove => _shouldCancelOnMove;
        public bool ShouldCancelOnDamage => _shouldCancelOnDamage;
        public bool ShouldCancelOnAction => _shouldCancelOnAction;
        public bool ShouldCancelOnTargetOutOfRange => _shouldCancelOnTargetOutOfRange;
    }
}
