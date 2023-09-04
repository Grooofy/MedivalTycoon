using UnityEngine;
using DG.Tweening;
using System;

public abstract class Props : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Animator _animator; 

    public Action MoveEnded;

    internal virtual void TryMoveTo(Transform endPoint)
    {
        if (endPoint == null)
        {
            return;
        }
        MoveTo(endPoint);
    }

    private void MoveTo(Transform endPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, _moveSpeed * Time.deltaTime);
        if (IsMinDistance(transform.position, endPoint.position))
        {
            _animator.Play("Spin");
            MoveEnded?.Invoke();
        }
    }

    private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }
}