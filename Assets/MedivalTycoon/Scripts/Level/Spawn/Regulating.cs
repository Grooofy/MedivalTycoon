using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    public Action<bool> Fulling;

    private Stack<Props> _spawnedProps = new Stack<Props>();
    private Stack<Props> _propses = new Stack<Props>();

    private bool _isEmptyPoint;
    private int _indexPoint = 0;
    private int _indexBarrel = 0;
    private Props _takedObject;


    public void AddProps(Props props)
    {
        props.MoveEnded += NextObject;
        _spawnedProps.Push(props);
    }

    public IEnumerator FillingPoints()
    {
        while (_isEmptyPoint || _spawnedProps.Count > 0)
        {
            FillingPoint();
            yield return new WaitForSeconds(0.5f);
        }
        CheckPoints();
    }

    public Props GiveAway()
    {
        if (_indexBarrel > 0)
        {
            _indexBarrel--;
            _indexPoint--;
            _points[_indexPoint].IsFill = false;
            return _propses.Pop();
        }

        return null;
    }

    private void FillingPoint()
    {
        _takedObject = _spawnedProps.Peek();
        _spawnedProps.Pop();
        StartCoroutine(_takedObject.TryMoveTo(_points[_indexPoint].transform));
        _points[_indexPoint].IsFill = true;
    }

    private void CheckPoints()
    {
        _isEmptyPoint = _points.FirstOrDefault(point => point.IsFill == false);
        Fulling?.Invoke(!_isEmptyPoint);
    }


    private void NextObject()
    {
        if (_indexPoint != _points.Count)
        {
            _takedObject.MoveEnded -= NextObject;
            _propses.Push(_takedObject);
            _indexBarrel++;
            _indexPoint++;
        }
    }
}
