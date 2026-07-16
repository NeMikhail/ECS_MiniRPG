using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Configs
{
    [CreateAssetMenu(fileName = ConfigAssetNames._playerConfigFileName, menuName = ConfigAssetNames._playerConfigMenuName)]
    public sealed class PlayerConfig : ScriptableObject
    {
        private static readonly float _defaultMoveSpeed = 5f;
        private static readonly float _defaultMaxHealth = 100f;

        [SerializeField] private PlayerSpawnStrategyConfig _initialSpawnStrategy;
        [SerializeField] private float _moveSpeed = _defaultMoveSpeed;
        [SerializeField] private float _maxHealth = _defaultMaxHealth;

        public PlayerSpawnStrategyConfig InitialSpawnStrategy => _initialSpawnStrategy;
        public float MoveSpeed => _moveSpeed;
        public float MaxHealth => _maxHealth;
    }
}
