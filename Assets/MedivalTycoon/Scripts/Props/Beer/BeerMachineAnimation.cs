using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BeerMachineAnimation : MonoBehaviour
{
    [Tooltip("Время одного полного цикла (увеличение + уменьшение) в секундах")]
    public float durationPerCycle = 1f;

    [Tooltip("Максимальный масштаб по Y, до которого будет увеличиваться объект")]
    public float maxScaleY = 2f; // например, удвоить

    public float maxScaleX = 2f; // например, удвоить

    private Vector3 originalScale;
    private Sequence scaleSequence;

    void Awake()
    {
       
        originalScale = transform.localScale;
    }

   

    void OnDisable()
    {
        // Останавливаем анимацию, чтобы не было «зависших» tweens
        if (scaleSequence != null)
            scaleSequence.Kill();
    }


    private void PlayAnimation(int loopCount)
    {
        if (scaleSequence != null && scaleSequence.IsActive())
            scaleSequence.Kill();

        scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform
            .DOScale(new Vector3(maxScaleX, maxScaleY, originalScale.z), durationPerCycle / 2f).SetEase(Ease.OutQuad));

        scaleSequence.Append(transform.DOScale(originalScale, durationPerCycle / 2f).SetEase(Ease.InQuad));

        scaleSequence.SetLoops(loopCount, LoopType.Restart);
        scaleSequence.Play();
    }
}