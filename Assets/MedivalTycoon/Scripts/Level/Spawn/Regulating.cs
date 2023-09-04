using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    private List<Props> _propses = new List<Props>();

    private Point _currentPoint;
    private int _indexBarrel = 0;


    private void Start()
    {
        StartCoroutine(FillinPoints());
    }

    public void AddProps(Props props)
    {
        _propses.Add(props);
    }


    private IEnumerator FillinPoints()
    {
        _currentPoint = _points[0];
        while (_indexBarrel < _points.Count)
        {
            GetProps().TryMoveTo(_currentPoint.transform);

            yield return new WaitForSeconds(10f);
        }
    }

    private Props GetProps()
    {
        if (_propses != null)
        {
            return _propses[_indexBarrel];
        }
        return null;
    }


}