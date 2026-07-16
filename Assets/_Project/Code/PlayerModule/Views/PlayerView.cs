using ECSMiniRPG.GUIModule.Views;
using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Views
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private Camera _camera;

        public Rigidbody Rigidbody => _rigidbody;
        public HealthBarView HealthBarView => _healthBarView;
        public Camera Camera => _camera;
    }
}