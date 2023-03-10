public interface IDestructible
{
    void Damage(float damage);
    float GetHealth();
    float GetMaxHealth();
    bool IsAllive();
}
