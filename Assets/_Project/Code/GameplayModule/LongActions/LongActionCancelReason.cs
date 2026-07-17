namespace ECSMiniRPG.GameplayModule.LongActions
{
    public enum LongActionCancelReason
    {
        None = 0,
        Command = 1,
        Move = 2,
        Damage = 3,
        Action = 4,
        TargetOutOfRange = 5
    }
}
