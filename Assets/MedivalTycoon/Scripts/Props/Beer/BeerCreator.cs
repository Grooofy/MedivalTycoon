using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BeerCreator : MonoBehaviour, IPropsMover
{
    public Action<bool> Fulling { get; set; }
    [SerializeField] private int _amountBarrelToBeer;
    [SerializeField] private List<Point> _points = new List<Point>();
    
    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointsProps = new Queue<Props>();
    private WaitForSeconds _wait = new WaitForSeconds(1f);
    private Props _currentProps;
    private int _index;
    private bool _isFull;
    private int _currentCountBeerPoint;


    public void CreatePoints(int cout, float offset)
    {
        throw new NotImplementedException();
    }

    public void RegisterProps(Queue<Props> props)
    {
        _currentCountBeerPoint = _amountBarrelToBeer;
        
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
        _props.Enqueue(props);
    }

    public IEnumerator FillingPoints()
    {
        if (_props.Count == 0) yield break;
        var temporaryQueue = new Queue<Props>();

        while (_isFull == false && _index < _currentCountBeerPoint)
        {
            if (_props.Count == 0) yield break;
            var prop = _props.Peek();
            if (prop == null) yield break;
            
            StartCoroutine(prop.TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;
            
            if (_index == _points.Count)
            {
                _index = _points.Count - 1;
                _currentCountBeerPoint = _amountBarrelToBeer;
                _isFull = true;
            }
            _pointsProps = new Queue<Props>(temporaryQueue.Reverse());
            yield return _wait;
        }

        if (_index == _currentCountBeerPoint)
        {
            _currentCountBeerPoint += _amountBarrelToBeer;
            Fulling?.Invoke(true);
        }
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
