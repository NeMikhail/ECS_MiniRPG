using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.LoggerModule.Events
{
    public struct LoggerMessageEvent : IEvent
    {
        public LoggerMessage _message;
    }
}