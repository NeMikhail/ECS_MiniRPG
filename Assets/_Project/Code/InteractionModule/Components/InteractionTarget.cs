using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InteractionModule.Components
{
    public struct InteractionTarget : IComponent
    {
        public GameWorld.Entity _value;
    }
}
