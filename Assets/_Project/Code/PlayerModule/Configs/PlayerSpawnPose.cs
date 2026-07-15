using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Configs
{
    public readonly struct PlayerSpawnPose
    {
        public static PlayerSpawnPose Identity => new PlayerSpawnPose(Vector3.zero, Quaternion.identity);

        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public PlayerSpawnPose(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
