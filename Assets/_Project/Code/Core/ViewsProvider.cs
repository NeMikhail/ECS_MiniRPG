using ECSMiniRPG.LocationModule;
using UnityEngine;

namespace ECSMiniRPG.Core
{
    public sealed class ViewsProvider : MonoBehaviour
    {
        [SerializeField] private LocationView _locationView;

        public LocationView LocationView => _locationView;
    }
}
