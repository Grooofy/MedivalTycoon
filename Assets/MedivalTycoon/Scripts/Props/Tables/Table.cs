using Beers;
using Events;
using MedivalTycoon;
using SeatSyst;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tables
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private LayerMask _waiterLayer;
        [SerializeField] private LayerMask _visitorLayer;
        [SerializeField] private float _resetDelay;
        public event UnityAction<int> PriceChanged;
        public event UnityAction LinedUp;
        public bool IsBuilt => Price <= 0;
        public int Price { get; private set; }

        private Seat _seat;
        private BeerTaker _beerTaker;
        private SeatInventory _inventory;
        private bool _isBuilding;
        private bool _isTakeEnable;

        public void Initialize(int startPrice)
        {
            Price = startPrice;
            _seat = GetComponentInChildren<Seat>();
            _beerTaker = GetComponentInChildren<BeerTaker>();
            _inventory = GetComponentInChildren<SeatInventory>();
            _seat.InventoryFulling += SwitchCheckHits;
        }

        public void SwitchCheckHits()
        {
            if (_isTakeEnable)
                _isTakeEnable = false;
            else
                _isTakeEnable = true;
        }

        public void InitializeBeerTaker()
        {
            if (_beerTaker != null)
                _beerTaker.Initialize(_inventory, _waiterLayer);
        }

        public void InitializeSeatSystem(IPropsPool propsPool)
        {
            if (_seat != null)
                _seat.Initialize(_visitorLayer, _inventory, propsPool, _resetDelay);
        }

        public void CheckHits()
        {
            if (_seat == null && _beerTaker == null) return;
           
            if (_isBuilding && _isTakeEnable)
            {
                _beerTaker.CheckHits();
            }

            _seat.CheckHits();

        }

        public void ReducePrice(int step)
        {
            Price = Mathf.Max(Price - step, 0);
            PriceChanged?.Invoke(Price);

            if (Price <= 0)
            {
                LinedUp?.Invoke();
                _isBuilding = true;
                _isTakeEnable = true;
                EventBus.Raise(new TableBuilt(_seat));
                EventBus.Raise(new SeatFreed(_seat));
            }
        }

        private void OnDestroy()
        {
            _seat.InventoryFulling -= SwitchCheckHits;
        }
    }
}