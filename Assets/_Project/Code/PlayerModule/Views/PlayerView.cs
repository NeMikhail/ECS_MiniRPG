using UnityEngine;

namespace ECSMiniRPG.PlayerModule.Views
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
    }
}
