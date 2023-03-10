using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class ScreenFader : MonoBehaviour, IScreenFader
{
    public CanvasGroup canvasGroup;

    private Sequence _sequence;

    private void Start()
    {
        canvasGroup.alpha = 1;
        Invoke(nameof(Hide), 0.5f);
    }

    private void Hide()
    {
        canvasGroup.DOFade(0, 0.5f);
    }

    public void FadeScreen(float time, Action onFaded, Action onCompleted = null)
    {
        _sequence?.Kill();

        _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() => canvasGroup.alpha = 0);
        _sequence.Append(canvasGroup.DOFade(1, time / 2));
        _sequence.AppendCallback(() => onFaded?.Invoke());
        _sequence.Append(canvasGroup.DOFade(0, time / 2));
        _sequence.AppendCallback(() => onCompleted?.Invoke());
    }
}
