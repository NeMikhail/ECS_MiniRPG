namespace ECSMiniRPG.InteractionModule.Events
{
    public enum InteractionFailReason
    {
        None = 0,
        NoTarget = 1,
        TargetUnavailable = 2,
        MissingStrategy = 3,
        RequirementsNotMet = 4
    }
}
