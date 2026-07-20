using ECSMiniRPG.MobModule.Strategies;
using UnityEngine;

namespace ECSMiniRPG.MobModule.Configs
{
    [CreateAssetMenu(fileName = ConfigAssetNames._mobConfigFileName, menuName = ConfigAssetNames._mobConfigMenuName)]
    public sealed class MobConfig : ScriptableObject
    {
        [SerializeField] private string _prefabKey = "";
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _armor = 0f;
        [SerializeField] private MobMovementBehaviourConfig _movementBehaviour;
        [SerializeField] private MobAttackBehaviourConfig _attackBehaviour;

        public string PrefabKey => _prefabKey;
        public float MaxHealth => _maxHealth;
        public float Armor => _armor;
        public MobMovementBehaviourConfig MovementBehaviour => _movementBehaviour;
        public MobAttackBehaviourConfig AttackBehaviour => _attackBehaviour;
        public bool HasPrefabKey => !string.IsNullOrWhiteSpace(_prefabKey);
    }
}


