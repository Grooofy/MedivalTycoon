using System;
using MedivalTycoon;
using Money;
using Propses;
using UnityEngine;


public class PropsSpawner : MonoBehaviour
{
    [SerializeField] private PropsPool<Barrel> _barrelPool;
    [SerializeField] private PropsPool<Beer> _beerPool;
    [SerializeField] private PropsPool<Coin> _coinPool;
    [SerializeField] private int _defaultSize;
    [SerializeField] private int _maxSize;

    private PropsPool<Barrel> _currentBarrelPool;
    private PropsPool<Beer> _currentBeerPool;
    private PropsPool<Coin> _currentCoinPool;

    public IPropsPool GetBarrelPool()
    {
        if (_currentBarrelPool == null)
            _currentBarrelPool = _barrelPool.Initialize(_defaultSize, _maxSize);

        return _currentBarrelPool;
    }

    public IPropsPool GetBeerPool()
    {
        if (_currentBeerPool == null)
            _currentBeerPool = _beerPool.Initialize(_defaultSize, _maxSize);

        return _currentBeerPool;
    }

    public IPropsPool GetCoinPool()
    {
        if (_currentCoinPool == null)
            _currentCoinPool = _coinPool.Initialize(_defaultSize, _maxSize);
       
        return _currentCoinPool;
    }
}