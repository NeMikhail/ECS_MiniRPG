using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.MobModule.Components
{
    public struct MobAttackState : IComponent
    {
        public float _nextAttackTime;

        public bool CanAttack(float time)
        {
            return time >= _nextAttackTime;
        }

        public void StartCooldown(float time, float cooldown)
        {
            _nextAttackTime = time + cooldown;
        }
    }
}
