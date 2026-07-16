using UnityEngine;

namespace ECSMiniRPG.GUIModule.Views
{
    public sealed class Billboard : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        private void LateUpdate()
        {
            if (_camera != null)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
            }
        }
    }
}
