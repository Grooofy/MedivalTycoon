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

    public PropsType Type => PropsType.Coin;



    internal void Initialize(IPropsPool coinsPool, Point finishPoint)
    {
        _coinPool = coinsPool;
        _finishPoint = finishPoint;
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

            _finishPoint.Free();
            props.Reset();
            _coinPool.Despawn(props);

            yield return WaitFor.TenthSecond;
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
