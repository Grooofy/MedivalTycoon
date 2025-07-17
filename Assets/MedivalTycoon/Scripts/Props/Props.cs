using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using Barrels;
using MedivalTycoon.Scripts.Events;


public abstract class Props : MonoBehaviour
{
    private Transform _startPoint;
    private BarrelAnimation _barrelAnimation;
    private float _moveSpeed;

    internal abstract IEnumerator TryMoveTo(Point endPoint);

    private const float _endValueScale = 1.5f;
    private const float _startValueScale = 1.0f;
    private const float _durationAnimation = 1f;


    public void Initilization(Transform parent, float moveSpeed)
    {
        _moveSpeed =  moveSpeed;
        _startPoint = parent;
        
    }

    public void BarrelEmpty()
    {
        ReturnScale();
    }

    public void Reset(IPropsMover propsMover, Point point)
    {
        point.IsFill = false;
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

    internal bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
    {
        float minDistance = 0.001f;
        return Vector3.Distance(startPosition, endPosition) < minDistance;
    }

    private void ReturnScale()
    {
        transform.DOScale(_startValueScale, _durationAnimation);
    }

    private void ResetAnimation()
    {
      //ВЫзов иветна
    }
}