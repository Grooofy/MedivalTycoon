using System;
using MedivalTycoon;
using Propses;
using UnityEngine;


public class PropsSpawner : MonoBehaviour
{
    [Header("BarrelPool")]
    [SerializeField] private PropsPool<Barrel> _barrelPool;
    [SerializeField] private PropsPool<Beer> _beerPool;
    [SerializeField] private int _defaultSize;
    [SerializeField] private int _maxSize;

    public IPropsPool GetBarrelPool()
    {
        return _barrelPool.Initialize(_defaultSize, _maxSize);
    }
    
    public IPropsPool GetBeerPool()
    {
        return _beerPool.Initialize(_defaultSize, _maxSize);
    }
}