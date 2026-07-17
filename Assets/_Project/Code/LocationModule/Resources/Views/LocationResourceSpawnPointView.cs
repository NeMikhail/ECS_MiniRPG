using ECSMiniRPG.LocationModule.Resources.Configs;
using UnityEngine;

namespace ECSMiniRPG.LocationModule.Resources.Views
{
    public sealed class LocationResourceSpawnPointView : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private LocationResourceSpawnPointConfig _config;

        public string Id => _id;
        public LocationResourceSpawnPointConfig Config => _config;
    }
}
