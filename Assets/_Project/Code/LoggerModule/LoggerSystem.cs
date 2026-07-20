using System.Collections.Generic;
using ECSMiniRPG.Core;
using ECSMiniRPG.LoggerModule.Configs;
using ECSMiniRPG.LoggerModule.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECSMiniRPG.LoggerModule
{
    public static class LoggerSystem
    {
        private static LoggerLevel _globalLevel = LoggerLevel.None;
        private static readonly Dictionary<EcsModuleType, LoggerLevel> _moduleLevels = new Dictionary<EcsModuleType, LoggerLevel>();

        public static void ApplyConfig(LoggerModuleConfig config)
        {
            Clear();

            if (config != null)
            {
                _globalLevel = config.GlobalLevel;

                for (var i = 0; i < config.ModuleRules.Count; i++)
                {
                    var rule = config.ModuleRules[i];

                    if (rule != null && rule.ModuleType != EcsModuleType.None)
                    {
                        _moduleLevels[rule.ModuleType] = rule.Level;
                    }
                }
            }
        }

        public static void Clear()
        {
            _globalLevel = LoggerLevel.None;
            _moduleLevels.Clear();
        }

        public static void Info(EcsModuleType moduleType, string message, Object context = null)
        {
            Log(moduleType, LoggerLevel.Info, message, context);
        }

        public static void Debug(EcsModuleType moduleType, string message, Object context = null)
        {
            Log(moduleType, LoggerLevel.Debug, message, context);
        }

        public static void Warning(EcsModuleType moduleType, string message, Object context = null)
        {
            Log(moduleType, LoggerLevel.Warning, message, context);
        }

        public static void Error(EcsModuleType moduleType, string message, Object context = null)
        {
            Log(moduleType, LoggerLevel.Error, message, context);
        }

        public static bool IsEnabled(EcsModuleType moduleType, LoggerLevel level)
        {
            var enabledLevel = GetEnabledLevel(moduleType);
            return enabledLevel != LoggerLevel.None && level <= enabledLevel;
        }

        private static LoggerLevel GetEnabledLevel(EcsModuleType moduleType)
        {
            var level = _globalLevel;

            if (_moduleLevels.ContainsKey(moduleType))
            {
                level = _moduleLevels[moduleType];
            }

            return level;
        }

        private static void Log(EcsModuleType moduleType, LoggerLevel level, string message, Object context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (IsEnabled(moduleType, level))
            {
                var loggerMessage = new LoggerMessage(moduleType, level, message, context, Time.frameCount, Time.time);
                if (GameWorld.IsWorldInitialized)
                {
                    GameWorld.SendEvent(new LoggerMessageEvent
                    {
                        _message = loggerMessage
                    });
                }

                WriteToConsole(loggerMessage);
            }
#endif
        }

        private static void WriteToConsole(LoggerMessage message)
        {
            var text = $"[{message._level}] [{message._moduleType}] [F:{message._frame}] {message._message}";

            if (message._level == LoggerLevel.Error)
            {
                UnityEngine.Debug.LogError(text, message._context);
            }

            if (message._level == LoggerLevel.Warning)
            {
                UnityEngine.Debug.LogWarning(text, message._context);
            }

            if (message._level == LoggerLevel.Debug || message._level == LoggerLevel.Info)
            {
                UnityEngine.Debug.Log(text, message._context);
            }
        }
    }
}