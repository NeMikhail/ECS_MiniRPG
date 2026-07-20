using ECSMiniRPG.Core;
using UnityEngine;

namespace ECSMiniRPG.LoggerModule
{
    public readonly struct LoggerMessage
    {
        public readonly EcsModuleType _moduleType;
        public readonly LoggerLevel _level;
        public readonly string _message;
        public readonly Object _context;
        public readonly int _frame;
        public readonly float _time;

        public LoggerMessage(EcsModuleType moduleType, LoggerLevel level, string message, Object context, int frame, float time)
        {
            _moduleType = moduleType;
            _level = level;
            _message = message;
            _context = context;
            _frame = frame;
            _time = time;
        }
    }
}