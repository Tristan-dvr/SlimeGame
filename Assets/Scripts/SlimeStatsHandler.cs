using System;
using Zenject;

public class SlimeStatsHandler
{
    private SlimeState _state;
    private SlimeConfig _config;
    private PriceConfig _priceConfig;
    private RewardHandler _rewardHandler;
    private SignalBus _signalBus;

    public SlimeStatsHandler(SlimeConfig config, GameState state, RewardHandler rewardHandler, SignalBus signalBus, PriceConfig priceConfig)
    {
        _config = config;
        _state = state.slimeState;
        _rewardHandler = rewardHandler;
        _signalBus = signalBus;
        _priceConfig = priceConfig;
    }

    public float GetHealth() => _config.slimeHealth + _config.slimeHealthPerLevel * _state.healthLevel;

    public float GetAttack() => _config.attack + _config.attackPerLevel * _state.attackLevel;

    public float GetAttackSpeed() => _config.attackSpeed + _config.attackSpeedPerLevel * _state.attackSpeedLevel;

    public bool TryRaiseHealth()
    {
        var level = _state.healthLevel;
        return TrySetLevel(
            (l) => _state.healthLevel = l, 
            level, 
            CalculatePrice(_priceConfig.healthPrice, _priceConfig.healthPricePerLevel, level));
    }

    public bool TryRaiseAttack()
    {
        var level = _state.attackLevel;
        return TrySetLevel(
            (l) => _state.attackLevel = l,
            level,
            CalculatePrice(_priceConfig.attackPrice, _priceConfig.attackPricePerLevel, level));
    }

    public bool TryRaiseAttackSpeed()
    {
        var level = _state.attackSpeedLevel;
        return TrySetLevel(
            (l) => _state.attackSpeedLevel = l,
            level,
            CalculatePrice(_priceConfig.attackSpeedPrice, _priceConfig.attackSpeedPricePerLevel, level));
    }

    private bool TrySetLevel(Action<int> setLevel, int level, int price)
    {
        if (_rewardHandler.TryConsume(price))
        {
            setLevel(level + 1);
            _signalBus.Fire<GameEvents.SlimeStatsChanged>();
        }
        return false;
    }

    public int CalculatePrice(int price, float perLevel, int level) => price + (int)(level * perLevel);
}
