using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;

    private Queue<Props> _propses = new Queue<Props>();
    private Regulating _regulating;
    private Props _currentBarrel;
    private Coroutine _moved;
    private int _indexPoint = 0;

    public void ReceiveObject(Regulating regulating)
    {
        _regulating = regulating;
        TryTakeObject();
    }

    public void RemoveObject(Transform point)
    {
        _propses.Dequeue().TryMoveTo(point);
    }

    public void Stop()
    {
        if (_moved == null)
        {
            return;
        }
        StopCoroutine(_moved);
    }

    private void TryTakeObject()
    {
        if (_indexPoint > _points.Count)
        {
            return;
        }
        _moved = StartCoroutine(GetObject());
    }

    private IEnumerator GetObject()
    {
        _currentBarrel = _regulating.GiveAway();
        _currentBarrel.MoveEnded += NextObject;
       
        if (_propses.Count == _points.Count)
        {
            _currentBarrel.MoveEnded -= NextObject;
            _currentBarrel = null;
        }
        while (_indexPoint < _points.Count && _currentBarrel != null)
        {
            StartCoroutine(_currentBarrel.TryMoveTo(_points[_indexPoint]));
            yield return null;
        }
    }

    private void NextObject()
    {
        _indexPoint++;
        _currentBarrel = _regulating.GiveAway();
        if (_currentBarrel == null)
        {
            return;
        }
        _currentBarrel.MoveEnded += NextObject;
        _propses.Enqueue(_currentBarrel);
    }

}
