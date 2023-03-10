using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public Ball ballPrefab;
    public DamageText damageTextPrefab;
    public HealthBar healthBarPrefab;
    public RectTransform uiRoot;

    public override void InstallBindings()
    {
        Container.BindFactory<Ball, Ball.Factory>()
            .FromPoolableMemoryPool<Ball, Ball.Pool>(binding => binding
                .WithInitialSize(3)
                .FromComponentInNewPrefab(ballPrefab)
                .UnderTransformGroup("Balls"));

        Container.BindFactory<DamageText, DamageText.Factory>()
            .FromPoolableMemoryPool<DamageText, DamageText.Pool>(binding => binding
                .WithInitialSize(3)
                .FromComponentInNewPrefab(damageTextPrefab)
                .UnderTransform(uiRoot.transform));

        Container.BindFactory<HealthBar, HealthBar.Factory>()
            .FromPoolableMemoryPool<HealthBar, HealthBar.Pool>(binding => binding
                .WithInitialSize(3)
                .FromComponentInNewPrefab(healthBarPrefab)
                .UnderTransform(uiRoot.transform));

        Container.BindInterfacesAndSelfTo<SlimeStatsHandler>().AsCached();
        Container.BindInterfacesAndSelfTo<RewardHandler>().AsCached();
        Container.BindInterfacesAndSelfTo<DamageUIHandler>().AsCached().NonLazy();
        Container.BindInterfacesAndSelfTo<HealthBarUIHandler>().AsCached().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelStageHandler>().AsCached().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelLoop>().AsCached().NonLazy();
    }
}
