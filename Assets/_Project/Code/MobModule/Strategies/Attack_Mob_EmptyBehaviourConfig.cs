using ECSMiniRPG.MobModule.Components;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Strategies
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobAttackEmptyFileName, menuName = ConfigAssetNames._mobAttackEmptyMenuName)]
    public sealed class Attack_Mob_EmptyBehaviourConfig : MobAttackBehaviourConfig
    {
        public override void Execute(MobAttackContext context, ref MobAttackState state)
        {
        }
    }
}


