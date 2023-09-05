using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;


    private List<Props> _FullPropses = new List<Props>();
    private Queue<Props> _queuePropses = new Queue<Props>();

    private int _indexPoint = 0;
    private int _indexBarrel = 0;
    private Props _curentObject;
    private Coroutine _filingPoints;


    private void Start()
    {
        _filingPoints = StartCoroutine(FillinPoints());
    }

    public void AddProps(Props props)
    {
        props.MoveEnded += NetxObject;
        _FullPropses.Add(props);
    }

    public Props GiveawayProps()
    {
        return _queuePropses.Dequeue();
    }

    private void NetxObject()
    {
        _queuePropses.Enqueue(_curentObject);
        _indexBarrel++;
        _indexPoint++;

        if (_indexBarrel == _FullPropses.Count)
        {
            _indexBarrel = 0;
        }
    }
    //Идея в том что двигает до точки сам направитель
    private IEnumerator Gived()
    {
        while (_queuePropses != null)
        {
            yield return null;
        }
    }

    private IEnumerator FillinPoints()
    {
        while (_indexBarrel < _points.Count)
        {
            _curentObject = _FullPropses[_indexBarrel];
            _curentObject.TryMoveTo(_points[_indexPoint].transform);
            yield return null;
        }
    }
}