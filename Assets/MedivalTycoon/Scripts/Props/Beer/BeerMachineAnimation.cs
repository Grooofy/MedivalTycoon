using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BeerMachineAnimation : MonoBehaviour
{
    private float _durationPerCycle = 1f;
    private float _maxScaleY = 0.7f; 
    private float _maxScaleX = 0.7f; 
    private Vector3 originalScale;
    private Sequence scaleSequence;
    private ParticleSystem _particleSystem;

    public void Initialize()
    {
        originalScale = transform.localScale;
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

   

    void OnDisable()
    {
        if (scaleSequence != null)
            scaleSequence.Kill();
    }


    public void PlayAnimation()
    {
        if (scaleSequence != null && scaleSequence.IsActive())
            scaleSequence.Kill();

        scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform
            .DOScale(new Vector3(_maxScaleX, _maxScaleY, originalScale.z), _durationPerCycle / 2f).SetEase(Ease.OutQuad));
        
        _particleSystem.Play();

        scaleSequence.Append(transform.DOScale(originalScale, _durationPerCycle / 2f).SetEase(Ease.InQuad));
    }
}