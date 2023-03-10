using UnityEngine;
using Zenject;

public class Enemy : Character
{
    public float attackSpeed = 1;
    public float damage = 1;
    public float health = 10;
    public float moveSpeed = 1;
    public bool isBoss = false;

    private Slime _slime;
    private SignalBus _signalBus;

    private float _health;
    private bool _insideAttackRange;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    [Inject]
    protected void Construct(Slime slime, SignalBus signalBus)
    {
        _slime = slime;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _health = GetMaxHealth();
        _insideAttackRange = false;
        transform.position = _startPosition;
    }

    private void Update()
    {
        RefreshInRange();
        if (!_insideAttackRange && _slime.IsAllive())
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void RefreshInRange()
    {
        _insideAttackRange = Utils.InRange(transform.position, _slime.transform.position, attackRange);
    }

    protected override void AttackClosestEnemy()
    {
        if (_insideAttackRange && _slime.IsAllive())
            _slime.Damage(damage);
    }

    protected override float GetAttackPeriod() => 1 / attackSpeed;

    public override void Damage(float damage)
    {
        _health -= damage;
        if (_health < 0)
            _health = 0;
        if (!IsAllive())
            gameObject.SetActive(false);

        _signalBus.Fire(new GameEvents.Damaged()
        {
            character = this,
            damage = damage,
        });
    }

    public override float GetHealth() => _health;

    public override float GetMaxHealth() => health;
}
