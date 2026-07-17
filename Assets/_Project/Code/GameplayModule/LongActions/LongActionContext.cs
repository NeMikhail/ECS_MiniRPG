namespace ECSMiniRPG.GameplayModule.LongActions
{
    public readonly struct LongActionContext
    {
        public readonly GameWorld.Entity _owner;
        public readonly GameWorld.Entity _target;

        public LongActionContext(GameWorld.Entity owner, GameWorld.Entity target)
        {
            _owner = owner;
            _target = target;
        }
    }
}
