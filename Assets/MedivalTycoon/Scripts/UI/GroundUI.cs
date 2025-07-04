using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f; // Время анимации

    private bool isFading = false;
    
    public void FadeIn()
    {
        if (isFading) return;
        isFading = true;
        StartCoroutine(FadeRoutine(0f, 1f));
    }
    
    public void FadeOut()
    {
        if (isFading) return;
        isFading = true;
        StartCoroutine(FadeRoutine(1f, 0f));
    }

    
    private IEnumerator FadeRoutine(float fromAlpha, float toAlpha)
    {
        float startTime = Time.unscaledTime;
        float endTime = startTime + fadeDuration;

        while (Time.unscaledTime < endTime)
        {
            float t = (Time.unscaledTime - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, t);
            yield return null;
        }

        canvasGroup.alpha = toAlpha;

        isFading = false;
    }
    protected void SwitchActivate(bool isActivate)
    {
        gameObject.SetActive(isActivate);
    }
}
