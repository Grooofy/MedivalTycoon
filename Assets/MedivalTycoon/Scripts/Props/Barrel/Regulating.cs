using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Regulating : MonoBehaviour, IPropsMover
{
    public Action<bool> Fulling;
    public Action<bool> PointFill;

    [SerializeField] private List<Point> _points = new List<Point>();

    private WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointsProps = new Queue<Props>();
    private List<Props> _usedProps = new List<Props>();

    private Props _currentProps;
    private int _index;
    private bool _isFull;

    public void RegisterProps(Queue<Props> props)
    {
        if (props == null) return;
        if (props.Count == 0) return;

        foreach (var prop in props)
        {
            if (prop == null) continue;
            _props.Enqueue(prop);
        }
    }

    public void RegisterProp(Props props)
    {
        if (props == null) return;
        _props.Enqueue(props);
    }


    public IEnumerator FillingPoints()
    {
        if (_props.Count == 0) yield break;

        while (_isFull == false && _index < _points.Count)
        {
            if (_props.Count == 0) yield break;

            Props prop = null;

            for (int i = 0; i < _props.Count; i++)
            {
                var currentProp = _props.ElementAt(i);
                if (!_usedProps.Contains(currentProp))
                {
                    prop = currentProp;
                    break;
                }
            }

            if (prop == null) yield break;

            _usedProps.Add(prop);
            StartCoroutine(prop.TryMoveTo(_points[_index]));
            _pointsProps.Enqueue(prop);
            _index++;
            Debug.Log(prop.name);
            if (_index == _points.Count)
            {
                _index = _points.Count - 1;
                Fulling?.Invoke(true);
                _isFull = true;
            }
            
            PointFill?.Invoke(true);
            yield return _wait;
        }
        _pointsProps = new Queue<Props>(_pointsProps.Reverse());
    }

    public Queue<Props> GetTo(int amount)
    {
        if (_pointsProps.Count == 0) return new Queue<Props>();

        if (amount > _pointsProps.Count)
        {
            _index = amount;
            amount = _pointsProps.Count;
        }

        var queue = new Queue<Props>();

        for (int i = 0; i < amount; i++)
        {
            queue.Enqueue(_pointsProps.Dequeue());

            int indexToFree = _index - 1;
            if (indexToFree >= 0 && indexToFree < _points.Count)
            {
                _points[indexToFree].Free();
            }

            if (_index > 0) _index--;

            if (_pointsProps.Count == 0)
            {
                _index = 0;
                _isFull = false;
                Fulling?.Invoke(false);
                ResetPoints();
            }
        }
        return queue;
    }

    private void ResetPoints()
    {
        foreach (var point in _points)
        {
            point.Free();
        }
    }
}