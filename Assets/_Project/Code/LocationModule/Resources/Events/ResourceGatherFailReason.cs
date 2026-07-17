namespace ECSMiniRPG.LocationModule.Resources.Events
{
    public enum ResourceGatherFailReason
    {
        None = 0,
        NoNearbyResource = 1,
        MissingRequiredTool = 2,
        AlreadyGathered = 3
    }
}
