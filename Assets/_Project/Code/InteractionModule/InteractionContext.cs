namespace ECSMiniRPG.InteractionModule
{
    public readonly struct InteractionContext
    {
        public readonly GameWorld.Entity _actor;
        public readonly GameWorld.Entity _target;

        public InteractionContext(GameWorld.Entity actor, GameWorld.Entity target)
        {
            _actor = actor;
            _target = target;
        }
    }
}
