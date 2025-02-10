using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Regulating : MonoBehaviour
{
    public Action<bool> Fulling;
    
    [SerializeField] private List<Point> _points = new List<Point>();
    private WaitForSeconds _wait = new WaitForSeconds(0.6f);
    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointsProps;// = new Queue<Props>();
    
    private Props _currentProps;
    private int _index = 0;

    public void RegisterProps(Queue<Props> props)
    {
        foreach (var prop in props)
        {
            _props.Enqueue(prop);
        }
        
    }


    public IEnumerator FillingPoints()
    {
        var temporaryQueue = new Queue<Props>();
        
        while (_index < _points.Count)
        {
            StartCoroutine(_props.Peek().TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;

            if (_index == _points.Count)
            {
                Fulling?.Invoke(true);
                _pointsProps = new Queue<Props>(temporaryQueue.Reverse());
            }
            yield return _wait;
        }
    }

    public Queue<Props> GetTo(int count)
    {
        if (_pointsProps.Count == 0) return null;

        var queue = new Queue<Props>();
        
        for (int i = 0; i < count; i++)
        {
            queue.Enqueue(_pointsProps.Dequeue());
        }
        return queue;
    }
}