using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    public Action<bool> Fulling;
    private List<Props> _spawnedProps = new List<Props>();

    private Stack<Props> _propses = new Stack<Props>();

    private int _indexPoint = 0;
    private int _indexBarrel = 0;
    private Props _takedObject;

    private void Start()
    {
        CheckPoints();
    }

    public void AddProps(Props props)
    {
        props.MoveEnded += NetxObject;
        _spawnedProps.Add(props);
    }

    public void FillinPoint()
    {
        if (!_points[_indexPoint].IsFill)
        {
            _takedObject = _spawnedProps[_indexBarrel];
            _spawnedProps.RemoveAt(_indexBarrel);
            StartCoroutine(_takedObject.TryMoveTo(_points[_indexPoint].transform));
            _points[_indexPoint].IsFill = true;
        }

        CheckPoints();
    }

    public Props GiveAway()
    {
        if (_propses.Count > 0)
        {
            _indexBarrel--;
            _indexPoint--;
            _points[_indexPoint].IsFill = false;
            return _propses.Pop();
        }

        return null;
    }

    private void CheckPoints()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i].IsFill == false)
            {
                Fulling?.Invoke(false);
               return;
            }
            if (i == _points.Count)
            {
                Fulling?.Invoke(true);
            }
        }
    }

    private void NetxObject()
    {
        if (_indexPoint != _points.Count)
        {
            _propses.Push(_takedObject);
            _takedObject.MoveEnded -= NetxObject;
            _indexBarrel++;
            _indexPoint++;
            FillinPoint();
        }
    }
}