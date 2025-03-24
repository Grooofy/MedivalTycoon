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
    private Vector3 _startPositionValue;
    private Quaternion _startRotationValue;
    private Vector3 _startScaleValue;
    
    internal abstract IEnumerator TryMoveTo(Point endPoint);
    internal abstract IEnumerator TryJumpTo(Point endPoint,float elapsedTime, float moveDuration);


    private const float _endValueScale = 1.5f;
    private const float _startValueScale = 1.0f;
    private const float _durationAnimation = 1f;
  

    private void OnEnable()
    {
        _startPositionValue = transform.position;
        _startRotationValue = transform.rotation;
        _startScaleValue = transform.localScale;
    }

    public void Reset(IPropsMover propsMover, Point point)
    {
        ReturnScale();
        propsMover.RegisterProp(this);
        ResetAnimation();
        point.IsFill = false;
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

    internal void JumpTo(Point endPoint,Vector3 startPosition ,float elapsedTime, float moveDuration)
    {
        if(endPoint == null) return;
        
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / moveDuration;

        
        float height = Mathf.Sin(t * Mathf.PI) * _parabolaHeight;
        transform.position = Vector3.Lerp(startPosition, endPoint.transform.position, t) + Vector3.up * height;
        
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
        transform.DOScale(_startValueScale, _durationAnimation);
    }
    
    private void ResetAnimation()
    {
        _animator.SetTrigger("Reset");
    }
}