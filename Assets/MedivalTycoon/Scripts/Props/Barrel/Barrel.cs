using Barrels;
using Propses;
using System.Collections;
using DG.Tweening;
using UnityEngine;


public class Barrel : MonoBehaviour, IProps
{
    private TransformMover _mover = new TransformMover();
    private Transform _startPoint;
    private BarrelAnimation _barrelAnimation;
    private float _moveSpeed;

    private ParticleSystem _smokeParticle;
    private Vector3 _originalScale;
    private Sequence _scaleSequence;
    private float _squashScaleMultiplierXZ = 1.5f;
    private float _squashDuration = 0.16f;
    private float _unsquashDuration = 0.18f;

    public void Initilization(Transform parent, float moveSpeed, Animator animator)
    {
        _barrelAnimation = new BarrelAnimation(animator);
        _moveSpeed = moveSpeed;
        _startPoint = parent;

        _smokeParticle = GetComponentInChildren<ParticleSystem>();
        _originalScale = transform.localScale;
    }

    public void Reset()
    {
        transform.position = _startPoint.position;
        _barrelAnimation.Reset();

        if (_scaleSequence != null)
        {
            _scaleSequence.Kill();
            _scaleSequence = null;
        }

        transform.localScale = _originalScale;
    }

    public IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint.IsFill) yield break;

        while (endPoint.IsFill == false)
        {
            _mover.MoveTo(transform, endPoint, _moveSpeed);
            yield return null;
        }

        _barrelAnimation.MoveEnd();

        PlaySquashAnimation();

        if (_smokeParticle != null)
            _smokeParticle.Play();
    }

    private void PlaySquashAnimation()
    {
        if (_scaleSequence != null && _scaleSequence.IsActive())
            _scaleSequence.Kill();

        var squashScale = new Vector3(_originalScale.x * _squashScaleMultiplierXZ, _originalScale.y, _originalScale.z * _squashScaleMultiplierXZ);

        _scaleSequence = DOTween.Sequence();
        _scaleSequence.Append(transform.DOScale(squashScale, _squashDuration).SetEase(Ease.OutQuad));
        _scaleSequence.Append(transform.DOScale(_originalScale, _unsquashDuration).SetEase(Ease.InQuad));
    }
}
