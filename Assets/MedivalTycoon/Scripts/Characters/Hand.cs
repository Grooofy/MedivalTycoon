using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Hand : MonoBehaviour
{
    public Action<bool> HandFulling;
    [SerializeField] private List<Point> _points;
    [HideInInspector] public bool IsFulling;

    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointProps; 
    private WaitForSeconds _wait = new WaitForSeconds(0.2f);
    private Props _currentProps;
    private int _index = 0;


    public void RegisterProps(Regulating regulating)
    {
        if (IsFulling) return;

        if (regulating == null) return;
        
        _props = regulating.GetTo(_points.Count);
    }


    public IEnumerator FillingPoints()
    {
        var temporaryQueue = new Queue<Props>();
        
        while (_props.Count > 0)
        {
            StartCoroutine(_props.Peek().TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;
            
            if (_index == _points.Count)
            {
                IsFulling = true;
                HandFulling?.Invoke(true);
                _pointProps = new Queue<Props>(temporaryQueue.Reverse());
            }

            yield return _wait;
        }
    }

    public Queue<Props> GetTo()
    {
        if (_points.Count == 0) return null;

        var queue = new Queue<Props>();
        
        for (int i = 0; i < _points.Count; i++)
        {
            queue.Enqueue(_pointProps.Dequeue());
        }
        return queue;
    }
}