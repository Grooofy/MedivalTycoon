using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Propses;
using UnityEngine;


public class BarrelBuffer : MonoBehaviour, IPropsMover
{  
    private WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private Queue<IProps> _props = new Queue<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private SpawnerPoints _spawnerPoints = new SpawnerPoints();
    private List<Point> _points;
    private Vector3 _spaceSize;
    private IProps _currentBarrel;
    private int _index;
    private int _amountPoint;
    private bool _isFull;
    private string _sourceId;

    public void Initialize(string sourceId, Vector3 spaceSize)
    {
        _sourceId = sourceId;
        _spaceSize = spaceSize;
    }
  
    public void CreatePoints(int cout, float offset)
    {
       _spawnerPoints.Initialize(cout, offset, transform);
       _points = _spawnerPoints.SpawnObjectsInCube(_spaceSize);
    }

    public void RegisterProps(Queue<IProps> props)
    {
        if (props == null) return;
        if (props.Count == 0) return;

        foreach (var prop in props)
        {
            if (prop == null) continue;
            _props.Enqueue(prop);
        }
    }

    public void RegisterProp(IProps barrel)
    {
        if (barrel == null) return;
        _props.Enqueue(barrel);
    }


    public IEnumerator FillingPoints()
    {
        if (_props.Count == 0 || _isFull) yield break;

        while (!_isFull && _props.TryDequeue(out var prop))
        {
            if (_index >= _amountPoint) break;

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

    public Queue<IProps> GetTo(int amount)
    {
        var result = new Queue<IProps>();

        for (int i = 0; i < amount && _pointsProps.Count > 0; i++)
        {
            var prop = _pointsProps.Pop();
            result.Enqueue(prop);

            int indexToFree = _index - 1;
            if (indexToFree >= 0 && indexToFree < _points.Count)
            {
                _points[indexToFree].Free();
            }

            if (_index > 0) _index--;
        }

        if (_pointsProps.Count == 0)
        {
            _index = 0;
            _isFull = false;
            EventBus.Raise(new PropsMoverFullingPointEvent(false, _sourceId));
            ResetPoints();
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