using UnityEngine;
using Zenject;

public class Slime : Character, IDestructible
{
    private float _health;

    private Ball.Factory _ballFactory;
    private SlimeStatsHandler _stats;
    private DiContainer _container;
    private SignalBus _signalBus;

    private float _lastDamagedTime;
    private Enemy _target;

    [Inject]
    protected void Construct(Ball.Factory ballFactory, SlimeStatsHandler stats, DiContainer container, SignalBus signalBus)
    {
        _ballFactory = ballFactory;
        _stats = stats;
        _container = container;
        _signalBus = signalBus;
    }

    private void Start()
    {
        InvokeRepeating(nameof(TryHeal), 0.5f, 0.5f);
        Initialize();
    }

    public void Initialize()
    {
        _health = GetMaxHealth();
        _target = null;
    }

    private void TryHeal()
    {
        if (Time.time - _lastDamagedTime > 2)
            _health = Mathf.Min(GetMaxHealth(), _health + 1);
    }

    protected override void AttackClosestEnemy()
    {
        if (_target == null || !_target.IsAllive())
            _target = FindClosestAlliveEnemy();

        if (_target != null && Utils.InRange(transform.position, _target.transform.position, attackRange))
            Attack(_target);
    }

    private void Attack(Enemy target)
    {
        var ball = _ballFactory.Create();
        ball.Launch(transform.position, target.transform, _stats.GetAttackSpeed() * attackRange * 1.5f, () =>
        {
            target.Damage(_stats.GetAttack());
        });
    }

    private Enemy FindClosestAlliveEnemy()
    {
        var enemies = _container.Resolve<Enemy[]>();
        Enemy closestEnemy = null;
        float closestSqrDistance = float.MaxValue;
        foreach (var enemy in enemies)
        {
            if (!enemy.IsAllive())
                continue;

            var direction = enemy.transform.position - transform.position;
            var sqrDistance = direction.sqrMagnitude;
            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    protected override float GetAttackPeriod() => 1 / _stats.GetAttackSpeed();

    public override void Damage(float damage)
    {
        _health -= damage;
        if (_health < 0)
            _health = 0;

        _signalBus.Fire(new GameEvents.Damaged()
        {
            character = this,
            damage = damage,
        });
        _lastDamagedTime = Time.time;
    }

    public override float GetHealth() => _health;

    public override float GetMaxHealth() => _stats.GetHealth();
}
