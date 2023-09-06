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
    private int _indexPoint = -1;

    public void ReceiveObject(Regulating regulating)
    {
        _regulating = regulating;
        TryTakeObject();
    }

    private void TryTakeObject()
    {
        if (_indexPoint > _points.Count)
        {
            return;
        }
        GetObject();
    }

    private void GetObject()
    {
        _indexPoint++;
        if (_propses.Count >= _points.Count)
        {
            _curentBarrel = null;
            return;
        }
        _curentBarrel = _regulating.GiveAway();
        _curentBarrel.MoveEnded += GetObject;
        _propses.Enqueue(_curentBarrel);
        StartCoroutine(_curentBarrel.TryMoveTo(_points[_indexPoint]));
    }

}
