namespace ECSMiniRPG.GameplayModule
{
    public enum HealthCommandType
    {
        None = 0,
        Damage = 1,
        DirectDamage = 2,
        Heal = 3,
        SetCurrent = 4,
        SetMax = 5,
        ChangeMax = 6,
        Restore = 7,
        Kill = 8
    }
}