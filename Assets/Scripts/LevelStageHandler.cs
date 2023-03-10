using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class LevelStageHandler : IInitializable
{
    private Level _level;
    private Slime _slime;
    private Camera _camera;
    private DiContainer _container;
    private SignalBus _signalBus;

    private int _currentStage = -1;

    public LevelStageHandler(Camera camera, Slime slime, Level level, DiContainer container, SignalBus signalBus)
    {
        _camera = camera;
        _slime = slime;
        _level = level;
        _container = container;
        _signalBus = signalBus;
    }

    public int CurrentStage => _currentStage;

    public int StagesCount => _level.stages.Length;

    public void Initialize()
    {
        _slime.transform.position = _level.startPoint.position;
    }

    public void SetStageActive(int stage, bool withAnimation = true)
    {
        if (_currentStage == stage)
            return;

        if (_currentStage != -1)
            DeactivateStage(_currentStage);

        for (int i = 0; i < _level.stages.Length; i++)
        {
            _level.stages[i].gameObject.SetActive(i == stage);
        }

        ActivateStage(stage, withAnimation);
        _currentStage = stage;
        OnStageChanged();
    }

    private void ActivateStage(int stage, bool withAnimation)
    {
        var levelStage = _level.stages[stage];
        var enemies = levelStage.GetComponentsInChildren<Enemy>(true);
        _container.Bind<Enemy[]>().FromInstance(enemies).AsCached();

        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            enemy.Initialize();
        }

        if (withAnimation)
        {
            var ease = Ease.InOutQuad;
            _slime.transform.DOMoveX(levelStage.startPoint.position.x, 1.5f).SetEase(ease);
            _camera.transform.DOMoveX(levelStage.transform.position.x + 10, 1.5f).SetEase(ease);
        }
        else
        {
            SetPosition(_slime.transform, levelStage.startPoint.position.x);
            SetPosition(_camera.transform, levelStage.transform.position.x + 10);
        }
    }

    private void OnStageChanged()
    {
        _signalBus.Fire<GameEvents.LevelStageChanged>();
    }

    private void SetPosition(Transform target, float x)
    {
        var position = target.position;
        position.x = x;
        target.position = position;
    }

    private void DeactivateStage(int stage)
    {
        _container.Unbind<Enemy[]>();
    }
}
