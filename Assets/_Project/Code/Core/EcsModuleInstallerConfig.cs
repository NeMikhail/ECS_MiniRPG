using System;
using UnityEngine;

namespace ECSMiniRPG.Core
{
    [Serializable]
    public sealed class EcsModuleInstallerConfig
    {
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private EcsModuleType _moduleType;

        public EcsModuleInstallerConfig()
        {
        }

        public EcsModuleInstallerConfig(EcsModuleType moduleType)
        {
            _moduleType = moduleType;
        }

        public bool IsEnabled => _isEnabled;
        public EcsModuleType ModuleType => _moduleType;
    }
}
