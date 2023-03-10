using System;
using UnityEngine;
using Zenject;

public class DamageUIHandler : IInitializable, IDisposable
{
    private SignalBus _signalBus;
    private DamageText.Factory _damageTextFactory;
    private Camera _camera;

    public DamageUIHandler(SignalBus signalBus, DamageText.Factory damageTextFactory, Camera camera)
    {
        _signalBus = signalBus;
        _damageTextFactory = damageTextFactory;
        _camera = camera;
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
        var text = _damageTextFactory.Create();
        var viewportPosition = _camera.WorldToViewportPoint(data.character.transform.position);
        text.Initialize(viewportPosition, data.damage);
    }
}
