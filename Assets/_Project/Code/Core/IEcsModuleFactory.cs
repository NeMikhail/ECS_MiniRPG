namespace ECSMiniRPG.Core
{
    public interface IEcsModuleFactory
    {
        void RegisterResources();
        void RegisterUpdateSystems();
        void RegisterFixedSystems();
        void Destroy();
    }
}
