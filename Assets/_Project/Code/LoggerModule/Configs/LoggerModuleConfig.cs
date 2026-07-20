using System.Collections.Generic;
using ECSMiniRPG.Core;
using UnityEngine;

namespace ECSMiniRPG.LoggerModule.Configs
{
    [CreateAssetMenu(fileName = ConfigAssetNames._loggerModuleConfigFileName, menuName = ConfigAssetNames._loggerModuleConfigMenuName)]
    public sealed class LoggerModuleConfig : ScriptableObject
    {
        [SerializeField] private LoggerLevel _globalLevel = LoggerLevel.None;
        [SerializeField] private List<LoggerModuleRule> _moduleRules = new List<LoggerModuleRule>();

        public LoggerLevel GlobalLevel => _globalLevel;
        public IReadOnlyList<LoggerModuleRule> ModuleRules => _moduleRules;

        public LoggerLevel GetLevel(EcsModuleType moduleType)
        {
            var level = _globalLevel;

            for (var i = 0; i < _moduleRules.Count; i++)
            {
                if (_moduleRules[i] != null && _moduleRules[i].ModuleType == moduleType)
                {
                    level = _moduleRules[i].Level;
                }
            }

            return level;
        }
    }
}