using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points = new List<Point>();
    
    private Queue<Props> _props = new Queue<Props>();
    private Props _currentProps;

    private int _index = 0;

    public void RegisterProps(Props props)
    {
        _props.Enqueue(props);
    }


    public void FillingPoints()
    {
        while (_index <= _points.Count)
        {
            StartCoroutine(_props.Dequeue().TryMoveTo(_points[_index++]));
        }
    }
}