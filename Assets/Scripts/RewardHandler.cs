using System;
using Zenject;

public class RewardHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private IStorage _storage;
    private RewardConfig _rewardConfig;

    public RewardHandler(SignalBus signalBus, IStorage storage, RewardConfig rewardConfig)
    {
        _signalBus = signalBus;
        _storage = storage;
        _rewardConfig = rewardConfig;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.Damaged>(OnDamaged);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.Damaged>(OnDamaged);
    }

    private void OnDamaged(GameEvents.Damaged data)
    {
        if (data.character is Enemy enemy && !data.character.IsAllive())
        {
            var reward = enemy.isBoss ? _rewardConfig.bossReward : _rewardConfig.reward;
            _storage.GetState().currency += reward;
            _storage.Save();
            _signalBus.Fire<GameEvents.CurrencyChanged>();
        }
    }

    public bool TryConsume(int currency)
    {
        if (_storage.GetState().currency >= currency)
        {
            _storage.GetState().currency -= currency;
            _storage.Save();
            _signalBus.Fire<GameEvents.CurrencyChanged>();
            return true;
        }
        return false;
    }
}
