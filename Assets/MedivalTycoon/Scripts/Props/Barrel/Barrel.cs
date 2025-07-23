using UnityEngine;
using DG.Tweening;
using System.Collections;
using Barrels;
using MedivalTycoon.Scripts.Events;
using Propses;


public class Barrel : MonoBehaviour, IProps
{
    private Transform _startPoint;
    private BarrelAnimation _barrelAnimation;
    private float _moveSpeed;

    private const float _endValueScale = 1.5f;
    private const float _startValueScale = 1.0f;
    private const float _durationAnimation = 1f;


    public void Initilization(Transform parent, float moveSpeed, Animator animator)
    {
        _barrelAnimation = new BarrelAnimation(animator);
        _moveSpeed =  moveSpeed;
        _startPoint = parent;
    }

    public void Reset(IPropsMover propsMover, Point point)
    {
        point.IsFill = false;
        transform.SetParent(_startPoint);
        transform.position = _startPoint.position;
        _barrelAnimation.Reset();
        propsMover.RegisterProp(this);
    }
    
    public IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint == null) yield break;
        
        while (endPoint.IsFill == false)
        {
            MoveTo(endPoint);
            yield return null;
        }
        transform.position = endPoint.transform.position;
        _barrelAnimation.MoveEnd();
    }

    private void MoveTo(Point endPoint)
    {
        if (endPoint == null) return;

        transform.position =
            Vector3.MoveTowards(transform.position, endPoint.transform.position, _moveSpeed * Time.deltaTime);
        transform.SetParent(endPoint.transform);

        if (IsMinDistance(transform.position, endPoint.transform.position))
        {
            EventBus.Raise(new PointFillingEvent());
            endPoint.Fill();
        }
    }

    private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }
}