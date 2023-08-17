using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;

    public Transform GetEmptyPoint()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i] == enabled)
            {
                _points[i].gameObject.SetActive(false);
                return _points[i];
            }
        }
        return null;
    }
}
