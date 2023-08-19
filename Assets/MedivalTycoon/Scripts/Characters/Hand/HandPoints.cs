using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoints : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    public Transform GetEmptyPoint()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i].enabled)
            {
               
                return _points[i].transform;
            }
        }
        return null;
    }
}
