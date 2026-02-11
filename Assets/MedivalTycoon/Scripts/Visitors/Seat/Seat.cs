using Events;
using MedivalTycoon;
using Money;
using SeatSyst;
using System;
using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;
using Visitors;

public class Seat : MonoBehaviour
{
    public Action InventoryFulling;
    private SeatPoint _seatPoint;
    private CoinBuffer _coinBuffer;
    private TableInteractionMode _tableInteraction;
    private TavernVisitor _visitor;
    private SeatInventory _inventory;
    private SeatUI _seatUI;
    private Coroutine _beerReset;
    private Queue<Point> _wayPoints;
    private float _pointDistanceWallet = 0.07f;
    private float _pointDistanceBeer = 0.1f;
    private int _beerDisplayAmount;
    private int _pointAmount = 100;
    private int _walletAmount;
    private bool _isEmpty = true;
    

    public void Initialize(CoinBuffer coinBuffer, TableInteractionMode tableInteractionMode, Queue<Point> wayPoint, LayerMask visitorMask, SeatInventory seatInventory, IPropsPool beerPool, float resetDelay)
    {
        _coinBuffer = coinBuffer;  
        _wayPoints = wayPoint;
        _tableInteraction = tableInteractionMode;

        _inventory = seatInventory;
        _inventory.Initialize(_tableInteraction, beerPool, resetDelay);
        

        _seatPoint = GetComponentInChildren<SeatPoint>();
        _seatUI = GetComponentInChildren<SeatUI>();

        _seatPoint.Initialize(visitorMask, this);
        _seatUI.Initialize(this);
        
        _inventory.NeedTextUpdate += UpdateBeerCount;
        _inventory.BeersEnded += SwitchVisitorSleep;
    } 
    

    public void CreateBigAmountPointToWallet()
    {
        _coinBuffer.CreatePoints(_pointAmount, _pointDistanceWallet);
    }

    public void VisitorSet(TavernVisitor visitor)
    {
        _tableInteraction.Switch();
        _visitor = visitor;
        _visitor.LeavingTavern += LeaveTavern;
        _beerDisplayAmount = _visitor.BeerAmount;
        _walletAmount =_beerDisplayAmount / 2;

        _coinBuffer.SetAmountVisitorWallet(_walletAmount);
        _inventory.CreatePoints(_beerDisplayAmount, _pointDistanceBeer);
        _seatUI.UpdateBeerDisplay(_beerDisplayAmount); 
        _isEmpty = false;
    }

    private void UpdateBeerCount(int count)
    {
        _beerDisplayAmount -= count;
        _seatUI.UpdateBeerDisplay(_beerDisplayAmount);

        if (_beerDisplayAmount == 0)
        {
            _visitor.ChangeState(StateEvent.Drink);

            if (_beerReset != null)
                StopCoroutine(_beerReset);

            _beerReset = StartCoroutine(_inventory.ResetBeer());
        }
    }

    public void OnVisitorLeftTavern()
    {
        if (_visitor != null)
            _visitor.LeavingTavern -= LeaveTavern;

        EventBus.Raise(new SeatFreed(this));
        _inventory.DeletePoints();
        _isEmpty = true;
        _visitor = null;        
    }  

    private void SwitchVisitorSleep()
    {
        _visitor.ChangeState(StateEvent.Sleep);
         StartCoroutine(_coinBuffer.FillingPoints());
    }


    private void LeaveTavern()
    {        
        _seatUI.UpdateBeerDisplay(0);
    }

   
    public Queue<Vector3> GetWay()
    {
        var way = new Queue<Vector3>();

        foreach (var point in _wayPoints)
            way.Enqueue(point.GetPostition());

        way.Enqueue(_seatPoint.GetPosition());
        return way;
    }

    public void CheckHits()
    {
        if(_isEmpty)
            _seatPoint?.CheckHits();
    }

    private void OnDestroy()
    {
        if (_seatPoint != null && _inventory != null)
        {
            _inventory.NeedTextUpdate -= UpdateBeerCount;
            _inventory.BeersEnded -= SwitchVisitorSleep;
        }
    }
}