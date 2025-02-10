using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class Regulating : MonoBehaviour
{
    public Action<bool> Fulling;
    [FormerlySerializedAs("isMain")] public bool IsMain;
    
    [SerializeField] private List<Point> _points = new List<Point>();
    private WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointsProps = new Queue<Props>();
    
    private Props _currentProps;
    private int _index = 0;
    private bool _isFull;

    public void RegisterProps(Queue<Props> props)
    {
        if(props == null) return;
        
        foreach (var prop in props)
        {
            _props.Enqueue(prop);
        }
    }


    public IEnumerator FillingPoints()
    {
        var temporaryQueue = new Queue<Props>();
        
        while (_isFull == false && _index < _points.Count)
        {
            StartCoroutine(_props.Peek().TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;

            if (_index == _points.Count)
            {
                _index = _points.Count - 1;
                Fulling?.Invoke(true);
                _isFull = true;
                _pointsProps = new Queue<Props>(temporaryQueue.Reverse());
            }
            yield return _wait;
        }
    }

    public Queue<Props> GetTo(int count)
    {
        if (_index == 0) return null;
        if(_pointsProps.Count == 0) return null;

        var queue = new Queue<Props>();
        
        for (int i = 0; i < count; i++)
        {
            queue.Enqueue(_pointsProps.Dequeue());
            _points[_index].IsFill = false;
            _index--;
            
            if (_index == 0)
            {
                _isFull = false;
                _index = 0;
            }
        }
        return queue;
    }
}