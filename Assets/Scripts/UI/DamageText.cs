using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class DamageText : MonoBehaviour, IPoolable<IMemoryPool>
{
    public TMP_Text text;
    public CanvasGroup canvasGroup;
    [Space]
    public Vector2 offset;
    public float flyHeight = 10;
    public float flyTime = 1f;

    private IMemoryPool _pool;

    public void Initialize(Vector2 viewportPosition, float damage)
    {
        canvasGroup.alpha = 1;
        var rectTransform = transform as RectTransform;
        rectTransform.anchorMin = viewportPosition;
        rectTransform.anchorMax = viewportPosition;
        rectTransform.anchoredPosition = offset;

        text.text = damage.ToString();

        rectTransform.DOKill();
        rectTransform.DOAnchorPosY(offset.y + flyHeight, flyTime);
        canvasGroup.DOFade(0, flyTime).OnComplete(Despawn);
    }

    private void Despawn() => _pool.Despawn(this);

    public void OnDespawned() { }
    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }

    public class Factory : PlaceholderFactory<DamageText> { }
    public class Pool : MonoPoolableMemoryPool<IMemoryPool, DamageText> { }
}
