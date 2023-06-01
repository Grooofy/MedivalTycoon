using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;
    [SerializeField] private float _speed;
    
    public UnityAction BarrelArrived;

    private int _pointNumber = 0;
    private bool _isMoved = true;

    public void MoveObject(GameObject barrel)
    {
        StartCoroutine(MovedToPoint(_points[_pointNumber].transform.position, barrel));
    }    

    private IEnumerator MovedToPoint(Vector3 _pointPosition, GameObject barrel)
    {
        while (_isMoved && Vector3.Distance(barrel.transform.position, _pointPosition) > 0.001f)
        {
            barrel.transform.position =
                Vector3.MoveTowards(barrel.transform.position, _pointPosition, _speed * Time.deltaTime);
            yield return null;
        }
        CalculatePoint();
    }

    private void CalculatePoint()
    {
        if (_pointNumber == _points.Count)
        {
            _isMoved = false;
            return;
        }  
        TurnOffPoint(_points[_pointNumber]);
        _pointNumber++;
        BarrelArrived?.Invoke();
    }

    private void TurnOffPoint(Point point)
    {
        point.SetActiveValue(false);
    }
}