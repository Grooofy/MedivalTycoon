using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public abstract class Props : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Animator _animator; 

    public Action MoveEnded;

    internal abstract IEnumerator TryMoveTo(Point endPoint);

    internal void MoveTo(Point endPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, _moveSpeed * Time.deltaTime);
        
        if (IsMinDistance(transform.position, endPoint.transform.position))
        {
            _animator.SetTrigger("Take");
            transform.SetParent(endPoint.transform);
            endPoint.IsFill = true;
            MoveEnded?.Invoke();
        }
    }

    internal bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }
}