using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEditor;

public abstract class Props : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _parabolaHeight;
    [SerializeField] private Animator _animator; 

    public Action MoveEnded;

    internal abstract IEnumerator TryMoveTo(Point endPoint);
    internal abstract IEnumerator TryJumpTo(Point endPoint,float elapsedTime, float moveDuration);

    internal void MoveTo(Point endPoint)
    {
        if(endPoint == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, _moveSpeed * Time.deltaTime);
        transform.SetParent(endPoint.transform);
        
        if (IsMinDistance(transform.position, endPoint.transform.position))
        {
            MoveEnd(endPoint);
        }
    }

    internal void JumpTo(Point endPoint,Vector3 startPosition ,float elapsedTime, float moveDuration)
    {
        if(endPoint == null) return;
        
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / moveDuration;

        
        float height = Mathf.Sin(t * Mathf.PI) * _parabolaHeight;
        transform.position = Vector3.Lerp(startPosition, endPoint.transform.position, t) + Vector3.up * height;
        
        if (IsMinDistance(transform.position, endPoint.transform.position))
        {
           MoveEnd(endPoint);
        }
    }
    

    internal bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }

    private void MoveEnd(Point endPoint) 
    {
        _animator.SetTrigger("Take");
        endPoint.Fill();
        MoveEnded?.Invoke();
    }
}