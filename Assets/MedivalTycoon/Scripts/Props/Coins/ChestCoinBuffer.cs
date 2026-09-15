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
    [SerializeField, Min(0.01f), Tooltip("Seconds per item before tavern upgrades. Lower is faster.")]
    private float _fillInterval = 0.1f;
    private IPropsPool _coinPool;
    private Stack<IProps> _props = new Stack<IProps>();
    private Point _finishPoint;
    private Wallet _wallet;
    private bool _isFilling;

    public PropsType Type => PropsType.Coin;

    internal void Initialize(int coinPrice, IPropsPool coinsPool, Point finishPoint, Wallet wallet)
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
        if (_isFilling) yield break;
        _isFilling = true;
        try
        {
            while (_props.Count > 0)
            {
                _props.TryPop(out var props);
                if (props == null) continue;

                // Do not release the coin until it reaches the chest.
                yield return props.TryMoveTo(_finishPoint);
                _wallet.StartAddCoins(10);
                _coinPool.Despawn(props);
                _finishPoint.Free();
                yield return WaitFor.Seconds(TavernUpgrades.FillSeconds(_fillInterval));
            }
        }
        finally
        {
            _isFilling = false;
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
