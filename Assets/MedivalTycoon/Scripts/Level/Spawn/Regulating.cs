using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    public Action<bool> Fulling; 

    private List<Props> _fullPropses = new List<Props>();
    private Queue<Props> _queuePropses = new Queue<Props>();

    private int _indexPoint = 0;
    private int _indexBarrel = 0;
    private Props _takedObject;

    private void Start()
    {
        IsFull(false);
    }


    public void AddProps(Props props)
    {
        props.MoveEnded += NetxObject;
        _fullPropses.Add(props);
    }

    public void FillinPoint()
    {
        if (_indexBarrel == _points.Count)
        {
            _queuePropses = new Queue<Props>(_queuePropses.Reverse());
            IsFull(true);
            return;
        }
        _takedObject = _fullPropses[_indexBarrel];
        _fullPropses.RemoveAt(_indexBarrel);
        StartCoroutine(_takedObject.TryMoveTo(_points[_indexPoint].transform));
        
    }

    public Props GiveAway()
    {
        if (_queuePropses.Count > 0)
        {
            _indexBarrel--;
            _indexPoint--;
            return _queuePropses.Dequeue();
        }
        IsFull(false);
        return null;
    }

    private void IsFull(bool value)
    {
        Fulling?.Invoke(value);
    }

    private void NetxObject()
    {
        _queuePropses.Enqueue(_takedObject);
        _takedObject.MoveEnded -= NetxObject;
        _indexBarrel++;
        _indexPoint++;
        FillinPoint();
    }

}