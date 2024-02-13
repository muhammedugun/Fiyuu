using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Explosive>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Catapult>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InLevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AmmoSelectionUI>().FromComponentInHierarchy().AsSingle();
    }
}
