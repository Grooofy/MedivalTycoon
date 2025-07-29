using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Propses;
using UnityEngine;
using UnityEngine.Events;


public class BeerCreator : MonoBehaviour, IPropsMover
{
    public UnityAction<bool> Fulling { get; set; }
    [SerializeField] private int _amountBarrelToBeer;
    [SerializeField] private List<Point> _points = new List<Point>();
    
    private Queue<IProps> _props = new Queue<IProps>();
    private Queue<IProps> _pointsProps = new Queue<IProps>();
    private WaitForSeconds _wait = new WaitForSeconds(1f);
    private Barrel _currentBarrel;
    private int _index;
    private bool _isFull;
    private int _currentCountBeerPoint;


    public void Initialize(string sourceId, BarrelPool barrelPool)
    {
        throw new NotImplementedException();
    }

    public void CreatePoints(int cout, float offset, Vector3 spaceSize)
    {
        throw new NotImplementedException();
    }

    public void RegisterProps(Stack<IProps> props)
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
    
    public int GetEmptyPointsCount()
    {
        var index = 0;

        foreach (var point in _points)
        {
            if (point.IsFill) index++;
        }

        return index;
    }

    public void RegisterProp(IProps barrel)
    {
        _props.Enqueue(barrel);
    }

    public IEnumerator FillingPoints()
    {
        if (_props.Count == 0) yield break;
        var temporaryQueue = new Queue<IProps>();

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
            _pointsProps = new Queue<IProps>(temporaryQueue.Reverse());
            yield return _wait;
        }

        if (_index == _currentCountBeerPoint)
        {
            _currentCountBeerPoint += _amountBarrelToBeer;
            Fulling?.Invoke(true);
        }
    }

    public Stack<IProps> GetTo(int amount)
    {
        if (_pointsProps.Count == 0) return new Stack<IProps>();

        if (amount > _pointsProps.Count)
        {
            _index = amount;
            amount = _pointsProps.Count;
        }

        var queue = new Stack<IProps>();

        for (int i = 0; i < amount; i++)
        {
            queue.Push(_pointsProps.Dequeue());

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

    public IProps GetTos()
    {
        throw new NotImplementedException();
    }

    private void ResetPoints()
    {
        foreach (var point in _points)
        {
            point.Free();
        }
    }
}
