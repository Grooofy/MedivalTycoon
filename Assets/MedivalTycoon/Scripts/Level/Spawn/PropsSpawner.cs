using System;
using Propses;
using UnityEngine;


public class PropsSpawner : MonoBehaviour
{
    [Header("BarrelPool")]
    [SerializeField] private BarrelPool _barrelPool;
    [SerializeField] private int _defaultSize;
    [SerializeField] private int _maxSize;

    public BarrelPool GetBarrelPool()
    {
        return _barrelPool.Initialize(_defaultSize, _maxSize);
    }
}