using System.Collections;
using System.Collections.Generic;
using MedivalTycoon;
using Propses;
using UnityEngine;


public class BarrelBuffer : MonoBehaviour, IPropsMover
{
    [SerializeField, Min(0.01f), Tooltip("Seconds per item before tavern upgrades. Lower is faster.")]
    private float _fillInterval = 1f;
    public PropsType Type => PropsType.Barrel;

    private Queue<IProps> _props = new Queue<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private readonly Dictionary<IProps, Coroutine> _arrivals = new Dictionary<IProps, Coroutine>();
    private SpawnerPoints _spawnerPoints = new SpawnerPoints();
    private IPropsPool _barrelPool;
    private List<Point> _points;
    private int _index;
    private int _amountPoint;
    private bool _isFull;
    private string _sourceId;


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

    public int GetEmptyPointsCount() => Mathf.Max(0, _amountPoint - _index);

    public IEnumerator FillingPoints()
    {
        while (_isFull == false)
        {
            if (_index >= _amountPoint) break;


            yield return WaitFor.Seconds(TavernUpgrades.FillSeconds(_fillInterval));
            var prop = _barrelPool.Spawn();

            _arrivals[prop] = StartCoroutine(prop.TryMoveTo(_points[_index]));

            _pointsProps.Push(prop);
            _index++;

            if (_index >= _amountPoint)
            {
                _isFull = true;
                EventBus.Raise(new PropsMoverFullingPointEvent(true));
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
            if (_arrivals.TryGetValue(prop, out var arrival))
            {
                StopCoroutine(arrival);
                _arrivals.Remove(prop);
            }
           
            result.Push(prop);

            if (_index > 0)
            {
                _index--;
                _points[_index].Free();
                _isFull = false;
                EventBus.Raise(new PropsMoverFullingPointEvent(false));
            }
            
            if (_pointsProps.Count == 0)
            {
                _index = 0;               
                ResetPoints();
            }
        }
        result = new Stack<IProps>(result);
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