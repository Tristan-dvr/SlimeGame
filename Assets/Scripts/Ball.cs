using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour, IPoolable<IMemoryPool>
{
    public float maxHeight = 1;
    public AnimationCurve flyPattern = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    private IMemoryPool _pool;

    public void Launch(Vector3 from, Transform target, float speed, Action onReached)
    {
        transform.position = from;
        StartCoroutine(FlyTo(target, speed, onReached));
    }

    private IEnumerator FlyTo(Transform target, float flySpeed, Action onReached)
    {
        var timer = 0f;
        var flyTime = (target.position - transform.position).magnitude / flySpeed;
        var start = transform.position;
        while (timer < flyTime)
        {
            var normalizedTime = timer / flyTime;
            var targetPosition = Vector3.Lerp(start, target.position, normalizedTime);
            targetPosition.y += flyPattern.Evaluate(normalizedTime) * maxHeight;

            transform.position = targetPosition;
            yield return null;
            timer += Time.deltaTime;
        }
        onReached?.Invoke();
        _pool.Despawn(this);
    }

    public void OnDespawned() { }

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public class Factory : PlaceholderFactory<Ball> { }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, Ball> { }
}
