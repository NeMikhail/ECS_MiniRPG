using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Views
{
    public sealed class LootObjectView : MonoBehaviour
    {
        [SerializeField] private GameObject _visualRoot;

        public GameObject VisualRoot => _visualRoot;
    }
}