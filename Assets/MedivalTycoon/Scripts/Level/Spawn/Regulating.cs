using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _test;

   

    private void Start()
    {
        MoveObject();
    }


    public void MoveObject()
    {
        
        
       
    }

    private IEnumerator DirectAnObject(GameObject barrel)
    {
        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i].enabled)
            {
                StartCoroutine(MovedToPoint(_points[i].transform.position, barrel));
                yield return null;
            }
            _points[i].SetActiveValue(false);
        }
    }

    

    private IEnumerator MovedToPoint(Vector3 _pointPosition, GameObject barrel)
    {
        while (Vector3.Distance(barrel.transform.position, _pointPosition) > 0.001f)
        {
            barrel.transform.position =
                Vector3.MoveTowards(barrel.transform.position, _pointPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private void TurnOffPoint(Point point)
    {
        point.SetActiveValue(false);
    }
}