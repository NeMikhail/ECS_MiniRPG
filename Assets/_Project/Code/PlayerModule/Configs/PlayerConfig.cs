using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Configs
{
    [CreateAssetMenu(fileName = ConfigAssetNames._playerConfigFileName, menuName = ConfigAssetNames._playerConfigMenuName)]
    public sealed class PlayerConfig : ScriptableObject
    {
        private static readonly float _defaultMoveSpeed = 5f;

        [SerializeField] private PlayerSpawnStrategyConfig _initialSpawnStrategy;
        [SerializeField] private float _moveSpeed = _defaultMoveSpeed;

        public PlayerSpawnStrategyConfig InitialSpawnStrategy => _initialSpawnStrategy;
        public float MoveSpeed => _moveSpeed;
    }
}
