using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.MobModule.Components
{
    public struct MobTarget : IComponent
    {
        public GameWorld.Entity _value;
        public bool _hasValue;

        public bool HasSameValue(GameWorld.Entity value)
        {
            return _hasValue && _value.Equals(value);
        }

        public void Set(GameWorld.Entity value)
        {
            _value = value;
            _hasValue = true;
        }

        public void Clear()
        {
            _value = default;
            _hasValue = false;
        }
    }
}