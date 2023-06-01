using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;
    [SerializeField] private float _speed;

    public int CountBarrel => _points.Count;
    public UnityAction BarrelArrived;

    private int _pointNumber = 0;
    private bool _isMoved = true;

    public void MoveObject(GameObject barrel)
    {
        StartCoroutine(MovedToPoint(_points[_pointNumber].transform.position, barrel));
    }    

    private IEnumerator MovedToPoint(Vector3 _pointPosition, GameObject barrel)
    {
        while (Vector3.Distance(barrel.transform.position, transform.position) > 0.001f)
        {
            barrel.transform.position =
                Vector3.MoveTowards(barrel.transform.position, transform.position, _speed * Time.deltaTime);
            yield return null;
        }
        
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
        if (_pointNumber == _points.Count - 1)
        {
            _isMoved = false;
            return;
        }
        
        _pointNumber++;
        BarrelArrived?.Invoke();
    }

    private void TurnOffPoint(Point point)
    {
        point.SetActiveValue(false);
    }
}