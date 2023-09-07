using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;

    private Queue<Props> _propses = new Queue<Props>();
    private Regulating _regulating;
    private Props _curentBarrel;
    private Coroutine _moved;
    private int _indexPoint = 0;

    public void ReceiveObject(Regulating regulating)
    {
        _regulating = regulating;
        TryTakeObject();
    }

    public void Stop()
    {
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
        _curentBarrel = _regulating.GiveAway();
        _curentBarrel.MoveEnded += NextObject;
       
        if (_propses.Count == _points.Count)
        {
            _curentBarrel.MoveEnded -= NextObject;
            _curentBarrel = null;
        }
        while (_curentBarrel != null)
        {
            StartCoroutine(_curentBarrel.TryMoveTo(_points[_indexPoint]));
            yield return null;
        }
    }

    private void NextObject()
    {
        _indexPoint++;
        _curentBarrel = _regulating.GiveAway();
        if (_curentBarrel == null)
        {
            return;
        }
        _curentBarrel.MoveEnded += NextObject;
        _propses.Enqueue(_curentBarrel);
    }

}
