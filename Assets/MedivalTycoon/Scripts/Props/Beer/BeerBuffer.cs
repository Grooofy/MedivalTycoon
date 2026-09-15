using Beers;
using Events;
using MedivalTycoon;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class BeerBuffer : MonoBehaviour, IPropsMover
{
    [SerializeField, Min(0.01f), Tooltip("Seconds per item before tavern upgrades. Lower is faster.")]
    private float _fillInterval = 0.25f;
    public bool IsTake { get; set; }

    public PropsType Type => PropsType.Beer;

    private List<Point> _points = new List<Point>();
    private SpawnerPoints _spawnerPoints = new SpawnerPoints();
    private Stack<IProps> _props = new Stack<IProps>();
    private Stack<IProps> _pointsProps = new Stack<IProps>();
    private readonly Dictionary<IProps, Coroutine> _arrivals = new Dictionary<IProps, Coroutine>();
    private int _index;
    private bool _isFilling;
    private int _currentCountBeerPoint;
    private int _amountPoint;
    private int _startAmountBeerToBarrel;
    private IPropsPool _beerPool;
    private BeerMachineAnimation _beerMachineAnimation;


    public void Initialize( IPropsPool beerPool, int amountBeerToBarrel)
    {
        _beerPool = beerPool;
        _startAmountBeerToBarrel = Mathf.Max(1, amountBeerToBarrel);
        _currentCountBeerPoint = 0;
        _beerMachineAnimation = GetComponentInChildren<BeerMachineAnimation>();
        _beerMachineAnimation.Initialize();
        EventBus.Subscribe<BeerCreated>(StartFilingPoints);
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

    public int GetEmptyPointsCount() => Mathf.Max(0, _amountPoint - _index - _props.Count);

    private void StartFilingPoints(BeerCreated beerCreated)
    {
        _currentCountBeerPoint += _startAmountBeerToBarrel + TavernUpgrades.BonusMugs;
        if (!_isFilling) StartCoroutine(FillingPoints());
    }

    public IEnumerator FillingPoints()
    {
        if (_isFilling) yield break;
        _isFilling = true;
        try
        {
            while (_props.Count > 0 || _currentCountBeerPoint > 0)
            {
                if (_index >= _amountPoint)
                {
                    yield return null;
                    continue;
                }
                IProps prop;
                if (_props.Count > 0) prop = _props.Pop();
                else
                {
                    prop = _beerPool.Spawn();
                    _currentCountBeerPoint--;
                    _beerMachineAnimation.PlayAnimation();
                }
                _arrivals[prop] = StartCoroutine(prop.TryMoveTo(_points[_index]));
                _pointsProps.Push(prop);
                _index++;
                yield return WaitFor.Seconds(TavernUpgrades.FillSeconds(_fillInterval));
            }
        }
        finally { _isFilling = false; }
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

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BeerCreated>(StartFilingPoints);
    }

   
}