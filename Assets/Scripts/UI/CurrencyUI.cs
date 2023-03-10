using System;
using TMPro;
using UnityEngine;
using Zenject;

public class CurrencyUI : MonoBehaviour, IInitializable, IDisposable
{
    public TMP_Text text;

    private SignalBus _signalBus;
    private IStorage _storage;

    [Inject]
    public void Construct(IStorage storage, SignalBus signalBus)
    {
        _storage = storage;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<GameEvents.CurrencyChanged>(RefreshCurrency);
        RefreshCurrency();
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.CurrencyChanged>(RefreshCurrency);
    }

    private void RefreshCurrency()
    {
        text.text = $"Coins x{_storage.GetState().currency}";
    }
}
