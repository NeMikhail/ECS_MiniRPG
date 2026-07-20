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
            ResolveCamera();

            if (_camera == null)
            {
                return;
            }

            Vector3 cameraDirection = _camera.transform.position - transform.position;
            if (cameraDirection.sqrMagnitude <= Mathf.Epsilon)
            {
                return;
            }

            Vector3 pitchDirection = Vector3.ProjectOnPlane(cameraDirection, Vector3.right);
            if (pitchDirection.sqrMagnitude <= Mathf.Epsilon)
            {
                return;
            }

            float pitch = Mathf.Atan2(-pitchDirection.y, pitchDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void ResolveCamera()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }
    }
}
