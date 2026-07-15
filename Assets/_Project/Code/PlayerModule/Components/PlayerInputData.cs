using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Components
{
    public struct PlayerInputData : IComponent
    {
        public Vector2 _moveDirection;
    }
}
