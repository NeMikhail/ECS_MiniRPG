using ECSMiniRPG.PlayerModule.Views;
using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.PlayerModule.Components
{
    public struct PlayerViewRef : IComponent
    {
        public PlayerView _value;
    }
}
