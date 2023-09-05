using UnityEngine;
using DG.Tweening;
using System;

public abstract class Props : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Animator _animator; 

    public Action MoveEnded;

    internal abstract void TryMoveTo(Transform endPoint);

    internal void MoveTo(Transform endPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, _moveSpeed * Time.deltaTime);
        
        if (IsMinDistance(transform.position, endPoint.position))
        {
            _animator.SetTrigger("Take");
            MoveEnded?.Invoke();
        }
    }

    private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }
}