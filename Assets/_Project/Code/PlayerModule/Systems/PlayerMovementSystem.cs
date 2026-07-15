using ECSMiniRPG.PlayerModule.Components;
using FFS.Libraries.StaticEcs;
using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Systems
{
    public struct PlayerMovementSystem : ISystem
    {
        private static readonly float _maxDirectionSqrMagnitude = 1f;

        public void Update()
        {
            foreach (var entity in GameWorld.Query<All<PlayerTag, PlayerInputData, PlayerMoveSpeed, PlayerViewRef>>().Entities())
            {
                ref readonly var inputData = ref entity.Read<PlayerInputData>();
                ref readonly var moveSpeed = ref entity.Read<PlayerMoveSpeed>();
                ref readonly var playerViewRef = ref entity.Read<PlayerViewRef>();

                var moveDirection = new Vector3(inputData._moveDirection.x, default, inputData._moveDirection.y);

                if (moveDirection.sqrMagnitude > _maxDirectionSqrMagnitude)
                {
                    moveDirection.Normalize();
                }

                var rigidbody = playerViewRef._value.Rigidbody;
                var targetPosition = rigidbody.position + moveDirection * moveSpeed._value * Time.fixedDeltaTime;
                rigidbody.MovePosition(targetPosition);
            }
        }
    }
}
