using System.Collections.Generic;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;


    private List<Props> _FullPropses = new List<Props>();
    private Queue<Props> _queuePropses = new Queue<Props>();

    private int _indexPoint = 0;
    private int _indexBarrel = 0;
    private Props _takedObject;


    private void Start()
    {
        FillinPoints();
    }

    public void AddProps(Props props)
    {
        props.MoveEnded += NetxObject;
        _FullPropses.Add(props);
    }

    public Props GiveAway()
    {
        if (_queuePropses.Count > 0)
        {
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
        FillinPoints();
    }

    private void FillinPoints()
    {
        if (_indexBarrel == _points.Count)
        {
            return;
        }
        _takedObject = _FullPropses[_indexBarrel];
        StartCoroutine(_takedObject.TryMoveTo(_points[_indexPoint].transform));
    }
}