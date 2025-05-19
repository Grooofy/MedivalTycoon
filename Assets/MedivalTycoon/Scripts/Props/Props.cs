using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;


public abstract class Props : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _parabolaHeight;
    [SerializeField] private Animator _animator; 

    public Action MoveEnded;
    private Transform _startPoint;
   
    
    internal abstract IEnumerator TryMoveTo(Point endPoint);


    private const float _endValueScale = 1.5f;
    private const float _startValueScale = 1.0f;
    private const float _durationAnimation = 1f;
  

    public void Initilization(Transform parent)
    {
        _startPoint = parent;
    }

    public void Reset(IPropsMover propsMover, Point point)
    {
        point.IsFill = false;
        ReturnScale();
        transform.SetParent(_startPoint);
        transform.position = _startPoint.position;
        ResetAnimation();
        propsMover.RegisterProp(this);
    }
    
    public void ScaleUp()
    {
        transform.DOScale(_endValueScale, _durationAnimation);
    }
    
    internal void MoveTo(Point endPoint)
    {
        if(endPoint == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, _moveSpeed * Time.deltaTime);
        transform.SetParent(endPoint.transform);
        
        if (IsMinDistance(transform.position, endPoint.transform.position))
        {
            MoveEndAnimation(endPoint);
        }
    }

    internal bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }

    private void MoveEndAnimation(Point endPoint) 
    {
        _animator.SetTrigger("Take");
        endPoint.Fill();
        MoveEnded?.Invoke();
    }

    private void ReturnScale()
    {
        transform.DOScale(_startValueScale, _durationAnimation).OnComplete(ResetAnimation);
    }
    
    private void ResetAnimation()
    {
        _animator.SetTrigger("Reset");
    }
}