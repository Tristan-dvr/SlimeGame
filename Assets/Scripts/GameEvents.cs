using Zenject;

public class GameEvents : Installer
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<Damaged>();
        Container.DeclareSignal<CurrencyChanged>();
        Container.DeclareSignal<SlimeStatsChanged>();
        Container.DeclareSignal<LevelStageChanged>();
    }

    public struct Damaged
    {
        public Character character;
        public float damage;
    }

    public struct CurrencyChanged { }
    public struct SlimeStatsChanged { }
    public struct LevelStageChanged { }
}
