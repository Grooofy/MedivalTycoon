using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MedivalTycoon;
using Propses;
using UnityEngine;


public class BarrelBuffer : MonoBehaviour, IPropsMover
{
    private WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private Queue<IProps> _props = new Queue<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private SpawnerPoints _spawnerPoints = new SpawnerPoints();
    private IPropsPool _barrelPool;
    private List<Point> _points;
    private int _index;
    private int _amountPoint;
    private bool _isFull;
    private string _sourceId;
    public bool IsTake { get; set; }


    public void Initialize(string sourceId, IPropsPool barrelPool)
    {
        _sourceId = sourceId;
        _barrelPool = barrelPool;
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

    public IEnumerator FillingPoints()
    {
        while (_isFull == false && IsTake)
        {
            if (_index >= _amountPoint) break;

            var prop = _barrelPool.Spawn();

            yield return prop.TryMoveTo(_points[_index]);

            _pointsProps.Push(prop);
            _index++;

            if (_index >= _amountPoint)
            {
                _isFull = true;
                EventBus.Raise(new PropsMoverFullingPointEvent(true, _sourceId));
            }

            yield return _wait;
        }
    }

    public Stack<IProps> GetTo(int amount)
    {
        var result = new Stack<IProps>();
        int itemsToTake = Mathf.Min(amount, _pointsProps.Count);

        for (int i = 0; i < itemsToTake; i++)
        {
            var prop = _pointsProps.Pop();
            if (prop == null) return null;
            
            result.Push(prop);

            if (_index > 0)
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