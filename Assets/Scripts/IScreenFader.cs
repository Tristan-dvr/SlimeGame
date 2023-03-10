using System;

public interface IScreenFader
{
    void FadeScreen(float time, Action onFaded, Action onCompleted = null);
}
