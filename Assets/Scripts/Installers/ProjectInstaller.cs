using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Install<GameEvents>();

        var storage = new PlayerPrefsStorage();
        Container.BindInterfacesTo<PlayerPrefsStorage>().FromInstance(storage).AsCached();
        Container.Bind<GameState>().FromMethod(storage.GetState).AsCached();
    }
}
