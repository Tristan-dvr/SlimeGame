using System;
using System.Linq;
using Zenject;

public class LevelLoop : IInitializable, IDisposable
{
    private Slime _slime;
    private DiContainer _container;
    private SignalBus _signalBus;
    private LevelStageHandler _stages;
    private IScreenFader _fader;

    public LevelLoop(Slime slime, DiContainer container, SignalBus signalBus, LevelStageHandler stages, IScreenFader fader)
    {
        _slime = slime;
        _container = container;
        _signalBus = signalBus;
        _stages = stages;
        _fader = fader;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.Damaged>(OnDamaged);
        _stages.SetStageActive(0);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.Damaged>(OnDamaged);
    }

    private void OnDamaged(GameEvents.Damaged data)
    {
        if (!data.character.IsAllive() && ((data.character is Enemy enemy && enemy.isBoss) || data.character == _slime))
        {
            _fader.FadeScreen(3, () =>
            {
                _stages.SetStageActive(0, false);
                _slime.Initialize();
            });
        }
        else if (_container.Resolve<Enemy[]>().All(e => !e.IsAllive()) && _stages.CurrentStage < _stages.StagesCount - 1)
        {
            _stages.SetStageActive(_stages.CurrentStage + 1);
        }
    }
}
