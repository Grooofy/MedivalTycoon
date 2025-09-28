using MedivalTycoon;
using SeatSyst;
using System;
using System.Collections;
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

    public void Initialize(LayerMask visitorMask, SeatInventory seatInventory, IPropsPool beerPool, float resetDelay)
    {
        _inventory = seatInventory;
        _inventory.Initialize(beerPool, resetDelay);

        _seatPoint = GetComponentInChildren<SeatPoint>();
        _seatUI = GetComponentInChildren<SeatUI>();

        _seatPoint.Initialize(visitorMask);
        _seatUI.Initialize(this);

        _seatPoint.VisitorSet += OnVisitorSet;        
        _inventory.NeedTextUpdate += UpdateBeerText;        
    }    

    private void OnVisitorSet(TavernVisitor visitor)
    {
        if (_visitor != null) return;
        _visitor = visitor;
        _beerDisplayAmount = _visitor.BeerAmount;
        _inventory.CreatePoints(_beerDisplayAmount, _pointDistance);
        _seatUI.UpdateBeerDisplay(_beerDisplayAmount);
    }

    private void UpdateBeerText(int count)
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

   
    public Vector3 GetPosition()
    {
        return _seatPoint.GetPosition();
    }

    public void CheckHits()
    {
        _seatPoint?.CheckHits();
    }

    private void OnDestroy()
    {
        if (_seatPoint != null && _inventory != null)
        {
            _inventory.NeedTextUpdate -= UpdateBeerText;
            _seatPoint.VisitorSet -= OnVisitorSet;
        }

    }
}