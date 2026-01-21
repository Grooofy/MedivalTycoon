using MedivalTycoon;
using Money;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestCoinManager : MonoBehaviour
{
    [SerializeField] private ChestCoinBuffer _chestCoinBuffer;
    [SerializeField] private CoinTaker _coinTaker;
    [SerializeField] private PropsSpawner _propsSpawner;
    [SerializeField] private Point _finishPoint;

    [SerializeField] private LayerMask _layerMask;

    private IPropsPool _coinsPool;


    public void Initialize()
    {
        _coinsPool = _propsSpawner.GetCoinPool();
        _chestCoinBuffer.Initialize(_coinsPool, _finishPoint);
        _coinTaker.Initialize(_chestCoinBuffer, _layerMask);
    }        

    public void CheckHits()
    {
        _coinTaker.CheckHits();
    }

}
