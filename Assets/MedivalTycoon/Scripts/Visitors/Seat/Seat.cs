using Events;
using MedivalTycoon;
using SeatSyst;
using System;
using UnityEngine;
using Visitors;

public class Seat : MonoBehaviour
{
    public Action InventoryFulling;
    private SeatPoint _seatPoint;
    private TavernVisitor _visitor;
    private SeatInventory _inventory;
    private SeatUI _seatUI;
    private float _pointDistance = 0.1f;
    private int _beerDisplayAmount;
    private Coroutine _beerReset;
    private bool _isEmpty = true;
    

    public void Initialize(LayerMask visitorMask, SeatInventory seatInventory, IPropsPool beerPool, float resetDelay)
    {
        _inventory = seatInventory;
        _inventory.Initialize(beerPool, resetDelay);

        _seatPoint = GetComponentInChildren<SeatPoint>();
        _seatUI = GetComponentInChildren<SeatUI>();

        _seatPoint.Initialize(visitorMask, this);
        _seatUI.Initialize(this);
        
        _inventory.NeedTextUpdate += UpdateBeerCount;
        _inventory.BeersEnded += SwitchVisitorSleep;
    } 
    

    public void VisitorSet(TavernVisitor visitor)
    {
        _visitor = visitor;
        _visitor.LeavingTavern += LeaveTavern;
        _beerDisplayAmount = _visitor.BeerAmount;
        _inventory.CreatePoints(_beerDisplayAmount, _pointDistance);
        _seatUI.UpdateBeerDisplay(_beerDisplayAmount);        
        EventBus.Subscribe<VisitorLeaveTavern>(OnVisitorLeftTavern);
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
            InventoryFulling?.Invoke();
        }
    }

    private void OnVisitorLeftTavern(VisitorLeaveTavern visitor)
    {
        if (_visitor != null)
            _visitor.LeavingTavern -= LeaveTavern;

        EventBus.Raise(new SeatFreed(this));
        _isEmpty = true;
        _visitor = null;
        EventBus.Unsubscribe<VisitorLeaveTavern>(OnVisitorLeftTavern);
    }

    private void SwitchVisitorSleep()
    {
        _visitor.ChangeState(StateEvent.Sleep);
    }


    private void LeaveTavern()
    {
        _seatUI.UpdateBeerDisplay(0);
        _inventory.DeletePoints();                
    }

   
    public Vector3 GetPosition()
    {  
        return _seatPoint.GetPosition();
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