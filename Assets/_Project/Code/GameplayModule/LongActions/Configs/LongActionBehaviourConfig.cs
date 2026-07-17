using UnityEngine;

namespace ECSMiniRPG.GameplayModule.LongActions
{
    public abstract class LongActionBehaviourConfig : ScriptableObject
    {
        public abstract void Complete(LongActionContext context);

        public virtual void Cancel(LongActionContext context, LongActionCancelReason reason)
        {
        }
    }
}
