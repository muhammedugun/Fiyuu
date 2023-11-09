using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<FiringBar>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Explosive>().FromComponentInHierarchy().AsSingle();
    }
}
