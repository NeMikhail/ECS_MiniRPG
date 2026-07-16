using FFS.Libraries.StaticEcs;

namespace ECSMiniRPG.GUIModule.Systems
{
    public struct GUISystem : ISystem
    {
        public void Update()
        {
            ref var controller = ref GameWorld.GetResource<GUIController>();
            controller.Update();
        }
    }
}
