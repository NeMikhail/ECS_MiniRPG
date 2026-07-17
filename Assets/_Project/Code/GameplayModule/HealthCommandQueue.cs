using ECSMiniRPG.GameplayModule.Events;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GameplayModule
{
    public sealed class HealthCommandQueue : IResource
    {
        public void EnqueueDamage(GameWorld.Entity target, float value)
        {
            Enqueue(HealthCommandType.Damage, target, value);
        }

        public void EnqueueDirectDamage(GameWorld.Entity target, float value)
        {
            Enqueue(HealthCommandType.DirectDamage, target, value);
        }

        public void EnqueueHeal(GameWorld.Entity target, float value)
        {
            Enqueue(HealthCommandType.Heal, target, value);
        }

        public void EnqueueSetCurrent(GameWorld.Entity target, float value)
        {
            Enqueue(HealthCommandType.SetCurrent, target, value);
        }

        public void EnqueueSetMax(GameWorld.Entity target, float value)
        {
            Enqueue(HealthCommandType.SetMax, target, value);
        }

        public void EnqueueChangeMax(GameWorld.Entity target, float value)
        {
            Enqueue(HealthCommandType.ChangeMax, target, value);
        }

        public void EnqueueRestore(GameWorld.Entity target)
        {
            Enqueue(HealthCommandType.Restore, target, 0f);
        }

        public void EnqueueKill(GameWorld.Entity target)
        {
            Enqueue(HealthCommandType.Kill, target, 0f);
        }

        private void Enqueue(HealthCommandType type, GameWorld.Entity target, float value)
        {
            GameWorld.SendEvent(new HealthCommandEvent
            {
                _type = type,
                _target = target,
                _value = value
            });
        }
    }
}