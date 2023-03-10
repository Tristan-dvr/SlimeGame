using System;
using UnityEngine;
using Zenject;

public class SlimeStatsUI : MonoBehaviour, IInitializable, IDisposable
{
    public SlimeStatsButton attack;
    public SlimeStatsButton attackSpeed;
    public SlimeStatsButton health;

    private SlimeStatsHandler _slimeStats;
    private SlimeConfig _config;
    private SlimeState _state;
    private PriceConfig _price;
    private SignalBus _signalBus;
    private IStorage _storage;

    private void Awake()
    {
        attack.button.onClick.AddListener(OnRaiseAttackClick);
        attackSpeed.button.onClick.AddListener(OnRaiseAttackSpeedClick);
        health.button.onClick.AddListener(OnRaiseHealthClick);
    }

    [Inject]
    public void Construct(SlimeStatsHandler slimeStats, SignalBus signalBus, SlimeConfig config, GameState state, PriceConfig price, IStorage storage)
    {
        _slimeStats = slimeStats;
        _signalBus = signalBus;
        _config = config;
        _state = state.slimeState;
        _price = price;
        _storage = storage;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.SlimeStatsChanged>(RefreshStats);
        _signalBus.Subscribe<GameEvents.CurrencyChanged>(RefreshStats);
        RefreshStats();
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.SlimeStatsChanged>(RefreshStats);
        _signalBus.Unsubscribe<GameEvents.CurrencyChanged>(RefreshStats);
    }

    private void RefreshStats()
    {
        attack.valueText.text = _slimeStats.GetAttack().ToString();
        attackSpeed.valueText.text = _slimeStats.GetAttackSpeed().ToString();
        health.valueText.text = _slimeStats.GetHealth().ToString();

        var currency = _storage.GetState().currency;

        var attackPrice = _slimeStats.CalculatePrice(_price.attackPrice, _price.attackPricePerLevel, _state.attackLevel);
        attack.priceText.text = FormatPrice(attackPrice);
        attack.button.interactable = currency >= attackPrice;

        var attackSpeedPrice = _slimeStats.CalculatePrice(_price.attackSpeedPrice, _price.attackSpeedPricePerLevel, _state.attackSpeedLevel);
        attackSpeed.priceText.text = FormatPrice(attackSpeedPrice);
        attackSpeed.button.interactable = currency >= attackSpeedPrice;

        var healthPrice = _slimeStats.CalculatePrice(_price.healthPrice, _price.healthPricePerLevel, _state.healthLevel);
        health.priceText.text = FormatPrice(healthPrice);
        health.button.interactable = currency >= healthPrice;

        attack.levelText.text = $"Lv {_state.attackLevel}";
        attackSpeed.levelText.text = $"Lv {_state.attackSpeedLevel}";
        health.levelText.text = $"Lv {_state.healthLevel}";
    }

    private string FormatPrice(int price) => $"Coins x{price}";

    private void OnRaiseAttackClick() => _slimeStats.TryRaiseAttack();
    private void OnRaiseAttackSpeedClick() => _slimeStats.TryRaiseAttackSpeed();
    private void OnRaiseHealthClick() => _slimeStats.TryRaiseHealth();
}
