using ECSMiniRPG.InteractionModule.Views;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.InteractionModule.Components
{
    public struct InteractionViewRef : IComponent
    {
        public InteractableView _value;
    }
}
