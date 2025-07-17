using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour, IPropsMover
{  
    private const string SourceId = "Barrel";
    [SerializeField] private List<Point> _points = new List<Point>();
    [SerializeField] private Seat _seat;

    private WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointsProps = new Queue<Props>();
    private List<Props> _usedProps = new List<Props>();

    private Props _currentProps;
    private int _index;
    private int _amountPoint;
    private bool _isFull;
    private TavernGuest _guest;
  
    public void CreatePoints(int cout, float offset)
    {
        throw new NotImplementedException();
    }

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
       
        if (_seat != null && _guest == null)
        {
            _guest = _seat.GetGuest();
            _amountPoint = _guest.GetBeerAmount();
            _wait = new WaitForSeconds(0.8f);
        }
        else
        {
            _amountPoint = _points.Count;
        }

        while (_isFull == false && _index < _amountPoint)
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
            
            if (_guest != null)
            {
                _guest.Drinking(_seat);
            }
            _pointsProps.Enqueue(prop);
            _index++;
            
            if (_index == _amountPoint)
            {
                _index = _amountPoint - 1;
                EventBus.Raise(new PropsMoverFullingPointEvent(true, SourceId));
                _isFull = true;
            }
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
                EventBus.Raise(new PropsMoverFullingPointEvent(false, SourceId));
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