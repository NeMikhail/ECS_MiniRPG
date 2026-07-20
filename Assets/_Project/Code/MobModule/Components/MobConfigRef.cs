using ECSMiniRPG.MobModule.Configs;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.MobModule.Components
{
    public struct MobConfigRef : IComponent
    {
        public MobConfig _value;
    }
}
