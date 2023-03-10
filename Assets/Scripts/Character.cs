using UnityEngine;

public abstract class Character : MonoBehaviour, IDestructible
{
    public float attackRange = 1;

    private float _attackTimer;

    protected abstract void AttackClosestEnemy();

    protected abstract float GetAttackPeriod();

    protected virtual void FixedUpdate()
    {
        if (!IsAllive())
            return;

        _attackTimer += Time.fixedDeltaTime;
        if (_attackTimer > GetAttackPeriod())
        {
            AttackClosestEnemy();
            _attackTimer = 0;
        }
    }

    public abstract void Damage(float damage);

    public abstract float GetHealth();

    public abstract float GetMaxHealth();

    public bool IsAllive() => GetHealth() > 0;
}
