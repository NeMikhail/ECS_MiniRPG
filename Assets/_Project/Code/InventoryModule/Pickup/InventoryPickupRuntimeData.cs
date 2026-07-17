using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InventoryModule.Pickup
{
    public sealed class InventoryPickupRuntimeData : IResource
    {
        private float _holdTime;
        private float _repeatTime;

        public float HoldTime => _holdTime;
        public float RepeatTime => _repeatTime;

        public void Reset()
        {
            _holdTime = 0f;
            _repeatTime = 0f;
        }

        public void AddHoldTime(float value)
        {
            _holdTime += value;
        }

        public void AddRepeatTime(float value)
        {
            _repeatTime += value;
        }

        public void ResetRepeatTime()
        {
            _repeatTime = 0f;
        }
    }
}
