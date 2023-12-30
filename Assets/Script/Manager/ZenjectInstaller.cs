using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<FiringBar>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Explosive>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InputRangeTest>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Catapult>().FromComponentInHierarchy().AsSingle();
        //Container.Bind<Trebuchet>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InLevelManager>().FromComponentInHierarchy().AsSingle();

    }
}
