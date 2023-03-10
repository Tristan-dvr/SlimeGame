using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthBar : MonoBehaviour, IPoolable<IMemoryPool>
{
    public Slider slider;
    public TMP_Text text;
    public Vector2 offset;

    private IMemoryPool _pool;

    public void Despawn() => _pool.Despawn(this);

    public void OnDespawned() { }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public class Factory : PlaceholderFactory<HealthBar> { }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, HealthBar> { }
}
