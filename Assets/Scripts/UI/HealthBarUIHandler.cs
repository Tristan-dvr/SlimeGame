using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class HealthBarUIHandler : IInitializable, ITickable, IDisposable
{
    private HealthBar.Factory _healthBarFactory;
    private DiContainer _container;
    private SignalBus _signalBus;
    private Slime _slime;
    private Camera _camera;

    private Dictionary<Character, HealthBar> _healthBars = new Dictionary<Character, HealthBar>();
    private Enemy[] _enemies;

    public HealthBarUIHandler(HealthBar.Factory healthBarFactory, Slime slime, DiContainer container, Camera camera, SignalBus signalBus)
    {
        _healthBarFactory = healthBarFactory;
        _slime = slime;
        _container = container;
        _camera = camera;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        RefreshCharacters();
        CreateHealthBar(_slime);

        _signalBus.Subscribe<GameEvents.LevelStageChanged>(RefreshCharacters);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.LevelStageChanged>(RefreshCharacters);
    }

    public void Tick()
    {
        RefreshHealthBars();
    }

    private void RefreshCharacters()
    {
        _enemies = _container.Resolve<Enemy[]>();
        foreach (var character in _healthBars.Keys.ToArray())
        {
            if (character is Slime)
                continue;
            RemoveHealthBar(character);
        }
    }

    private void RefreshHealthBars()
    {
        Enemy enemy;
        for (int i = 0; i < _enemies.Length; i++)
        {
            enemy = _enemies[i];
            var shouldShow = ShouldShowHealthBar(enemy);
            var contains = _healthBars.ContainsKey(enemy);
            if (!contains && shouldShow)
            {
                CreateHealthBar(enemy);
            }
            else if (contains && !shouldShow)
            {
                RemoveHealthBar(enemy);
            }
        }
        foreach (var character in _healthBars.Keys)
            RefreshHealthBar(character);
    }

    private bool ShouldShowHealthBar(Enemy enemy)
    {
        return enemy.IsAllive() && (enemy.isBoss || enemy.GetHealth() < enemy.GetMaxHealth());
    }

    private void RefreshHealthBar(Character character)
    {
        var viewportPositon = _camera.WorldToViewportPoint(character.transform.position);
        var bar = _healthBars[character];
        var rectTransform = bar.transform as RectTransform;
        rectTransform.anchorMin = rectTransform.anchorMax = viewportPositon;
        rectTransform.anchoredPosition = bar.offset;

        bar.slider.maxValue = character.GetMaxHealth();
        bar.slider.value = character.GetHealth();
        bar.text.text = character.GetHealth().ToString();
    }

    private void CreateHealthBar(Character character)
    {
        var bar = _healthBarFactory.Create();
        _healthBars.Add(character, bar);
        RefreshHealthBar(character);
    }

    private void RemoveHealthBar(Character character)
    {
        _healthBars[character].Despawn();
        _healthBars.Remove(character);
    }
}
