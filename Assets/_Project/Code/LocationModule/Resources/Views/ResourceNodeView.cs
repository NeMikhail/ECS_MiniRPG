using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Views
{
    public sealed class ResourceNodeView : MonoBehaviour
    {
        [SerializeField] private GameObject _activeRoot;
        [SerializeField] private GameObject _depletedRoot;

        public void ApplyState(LocationResourceNodeState state)
        {
            var isActive = state == LocationResourceNodeState.Active;
            var isDepleted = state == LocationResourceNodeState.Depleted;

            if (_activeRoot != null)
            {
                _activeRoot.SetActive(isActive);
            }

            if (_depletedRoot != null)
            {
                _depletedRoot.SetActive(isDepleted);
            }
        }
    }
}
