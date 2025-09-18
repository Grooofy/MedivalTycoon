using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
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
    private bool _isFilling;
    private int _currentCountBeerPoint;
    private int _amountPoint;
    private int _startAmountBeerToBarrel;
    private int currentAmountBeerToBarrel;
    private string _sourceId;
    private Coroutine _filingCoroutine;
    private IPropsPool _beerPool;
    private BeerMachineAnimation _beerMachineAnimation;


    public void Initialize(string sourceId, IPropsPool beerPool, int amountBeerToBarrel)
    {
        _sourceId = sourceId;
        _beerPool = beerPool;
        _startAmountBeerToBarrel = amountBeerToBarrel;
        _currentCountBeerPoint = _startAmountBeerToBarrel;
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

    public int GetEmptyPointsCount()
    {
        var index = 0;

        foreach (var point in _points)
        {
            if (point.IsFill == false) index++;
        }
        
        return index;
    }

    private void StartFilingPoints(BeerCreated beerCreated)
    {
        if (_isFilling) return; 

        _filingCoroutine = StartCoroutine(FillingPoints());
    }
    
    public IEnumerator FillingPoints()
    {
        while (_isFull == false && _currentCountBeerPoint > 0)
        {
            _isFilling = true;

            var prop = _beerPool.Spawn();

            yield return prop.TryMoveTo(_points[_index]);

            _pointsProps.Push(prop);
            _index++;
            _beerMachineAnimation.PlayAnimation();
            _currentCountBeerPoint--;
            
            if (_index >= _amountPoint)
                _isFull = true;
            yield return new WaitForSeconds(0.2f);
        }
        _isFilling = false;
        _filingCoroutine = null;
        _currentCountBeerPoint = _startAmountBeerToBarrel;
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
                _isFull = false;
            }
            
            if (_pointsProps.Count == 0)
            {
                _index = 0;
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

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BeerCreated>(StartFilingPoints);
    }
}