using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MedivalTycoon;
using Propses;
using UnityEngine;
using UnityEngine.Events;


public class BeerBuffer : MonoBehaviour, IPropsMover
{
    public bool IsTake { get; set; }
    private List<Point> _points = new List<Point>();
    private SpawnerPoints _spawnerPoints = new  SpawnerPoints();
    private Stack<IProps> _props = new Stack<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private int _index;
    private bool _isFull;
    private int _currentCountBeerPoint;
    private int _amountPoint;
    private string _sourceId;
    private IPropsPool _beerPool;


    public void Initialize(string sourceId, IPropsPool beerPool)
    {
        _sourceId = sourceId;
        _beerPool = beerPool;
        
    }

    public void CreatePoints(int cout, float offset, Vector3 spaceSize)
    {
        _spawnerPoints.Initialize(cout, offset, transform);
        _points = _spawnerPoints.SpawnObjectsInCube(spaceSize);
        _amountPoint = _points.Count;
    }

    public void RegisterProps(Stack<IProps> props)
    {
        if (props == null) return;
        if (props.Count == 0) return;

        foreach (var prop in props)
        {
            if (prop == null) continue;
            _props.Push(prop);
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

    
    public IEnumerator FillingPoints()
    {
        while (_isFull == false)
        {
            if (_index >= _amountPoint) break;

            var prop = _beerPool.Spawn();

            yield return prop.TryMoveTo(_points[_index]);

            _pointsProps.Push(prop);
            _index++;

            if (_index >= _amountPoint)
            {
                _isFull = true;
                EventBus.Raise(new PropsMoverFullingPointEvent(true, _sourceId));
            }
        }
    }


    public Stack<IProps> GetTo(int amount)
    {
        var result = new Stack<IProps>();
        int itemsToTake = Mathf.Min(amount, _pointsProps.Count);

        for (int i = 0; i < itemsToTake; i++)
        {
            _pointsProps.TryPop(out var prop);
           
            result.Push(prop);

            if (_index >= 0)
            {
                _index--;
                _points[_index].Free();
            }
            
            if (_pointsProps.Count == 0)
            {
                _index = 0;
                _isFull = false;
              
                ResetPoints();
            }
        }
        return result;
    }

    private void ResetPoints()
    {
        foreach (var point in _points)
        {
            point.Free();
        }
    }
}