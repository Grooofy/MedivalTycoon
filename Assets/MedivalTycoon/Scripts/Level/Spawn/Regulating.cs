using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;


    private List<Props> _fullPropses = new List<Props>();
    private Queue<Props> _queuePropses = new Queue<Props>();

    private int _indexPoint = 0;
    private int _indexBarrel = 0;
    private Props _takedObject;

    

    public void AddProps(Props props)
    {
        props.MoveEnded += NetxObject;
        _fullPropses.Add(props);
    }

    public void FillinPoint()
    {
        if (_indexBarrel == _points.Count)
        {
            _queuePropses  = new Queue<Props>(_queuePropses.Reverse());
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
        return null;
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