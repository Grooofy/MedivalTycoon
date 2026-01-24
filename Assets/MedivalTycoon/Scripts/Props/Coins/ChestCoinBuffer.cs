using Beers;
using Events;
using MedivalTycoon;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class ChestCoinBuffer : MonoBehaviour, IPropsMover
{
    private IPropsPool _coinPool;
    private Stack<IProps> _props = new Stack<IProps>();
    private Point _finishPoint;
    private Wallet _wallet;

    public PropsType Type => PropsType.Coin;



    internal void Initialize(IPropsPool coinsPool, Point finishPoint, Wallet wallet)
    {
        _coinPool = coinsPool;
        _finishPoint = finishPoint;
        _wallet = wallet;
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

    public IEnumerator FillingPoints()
    {
        while (_props.Count > 0)
        {
            _props.TryPop(out var props);
            if (props == null) break;

            StartCoroutine(props.TryMoveTo(_finishPoint));
            _wallet.StartAddCoins(30);
            yield return WaitFor.QuarterSecond;
            _coinPool.Despawn(props);
            props.Reset();
            _finishPoint.Free();
        }
    }

    public void CreatePoints(int cout, float offset, Vector3 spaceSize = default)
    {
        throw new System.NotImplementedException();
    }

    public int GetEmptyPointsCount()
    {
        throw new System.NotImplementedException();
    }

    public Stack<IProps> GetTo(int amount)
    {
        throw new System.NotImplementedException();
    }




}
