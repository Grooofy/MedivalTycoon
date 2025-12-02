using System.Collections;
using UnityEngine;

public class GroundUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private float _fadeDuration = 1f; 
    private bool _isFading = false;

    public void Initialize()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void FadeIn()
    {
        if (_isFading) return;
        _isFading = true;
        StartCoroutine(FadeRoutine(0f, 1f));
    }
    
    public void FadeOut()
    {
        if (_isFading) return;
        _isFading = true;
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    
    private IEnumerator FadeRoutine(float fromAlpha, float toAlpha)
    {
        float startTime = Time.unscaledTime;
        float endTime = startTime + _fadeDuration;

        while (Time.unscaledTime < endTime)
        {
            float t = (Time.unscaledTime - startTime) / _fadeDuration;
            _canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            yield return null;
        }

        _canvasGroup.alpha = toAlpha;

        _isFading = false;
    }
}
