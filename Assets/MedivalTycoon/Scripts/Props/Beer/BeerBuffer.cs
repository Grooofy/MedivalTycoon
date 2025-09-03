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
    private List<Point> _points = new List<Point>();
    private SpawnerPoints _spawnerPoints = new  SpawnerPoints();
    private Stack<IProps> _props = new Stack<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private int _index;
    private bool _isFull;
    private int _currentCountBeerPoint;
    private int _amountPoint;
    private string _sourceId;
    private int _amountBeerToBarrel;
    private IPropsPool _barrelPool;


    public void Initialize(string sourceId, IPropsPool barrelPool, int amountBarrelToBeer)
    {
        _sourceId = sourceId;
        _barrelPool = barrelPool;
        _amountBeerToBarrel = amountBarrelToBeer;
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
        while (_isFull == false && _props.Count > 0)
        {
            if (_index >= _amountPoint) break;
            
                
            _props.TryPop(out var props);
            if (props == null) break;
                
            yield return  props.TryMoveTo(_points[_index]);

            _pointsProps.Push(props);
            _index++;

            if (_index >= _amountPoint)
            {
                _index = _amountPoint;
                _isFull = true;
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
                EventBus.Raise(new PropsMoverFullingPointEvent(false, _sourceId));
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