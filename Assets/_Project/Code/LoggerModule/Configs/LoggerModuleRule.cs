using System;
using ECSMiniRPG.Core;
using UnityEngine;

namespace ECSMiniRPG.LoggerModule.Configs
{
    [Serializable]
    public sealed class LoggerModuleRule
    {
        [SerializeField] private EcsModuleType _moduleType;
        [SerializeField] private LoggerLevel _level = LoggerLevel.None;

        public EcsModuleType ModuleType => _moduleType;
        public LoggerLevel Level => _level;
    }
}