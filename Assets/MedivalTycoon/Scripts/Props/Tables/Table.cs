using System;
using Beers;
using Events;
using SeatSyst;
using UnityEngine;
using UnityEngine.Events;

namespace Tables
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private LayerMask _waiterLayer;
        [SerializeField] private LayerMask _visitorLayer;
        public event UnityAction<int> PriceChanged;
        public event UnityAction LinedUp;
        public bool IsBuilt => Price <= 0;
        public int Price { get; private set; }

        private Seat _seat;
        private BeerTaker _beerTaker;
        private SeatInventory _inventory;
        private bool _isBuilding;


        public void Initialize(int startPrice)
        {
            Price = startPrice;
            _seat = GetComponentInChildren<Seat>();           
            _beerTaker = GetComponentInChildren<BeerTaker>();
            _inventory = GetComponentInChildren<SeatInventory>();
        }

        public void InitializeBeerTaker()
        {
            if (_beerTaker != null)
                _beerTaker.Initialize(_inventory, _waiterLayer);
        }

        public void InitializeSeatSystem()
        {
            if(_seat != null)
            {
                _seat.Initialize(_visitorLayer, _inventory);               
            } 
        }

        public void CheckHits()
        {
            if (_beerTaker != null && _seat != null && _isBuilding)
            {
                _beerTaker.CheckHits();
                _seat.CheckHits();
            }             
        }

        public void ReducePrice(int step)
        {
            Price = Mathf.Max(Price - step, 0);
            PriceChanged?.Invoke(Price);

            if (Price <= 0)
            {
                LinedUp?.Invoke();
                _isBuilding = true;
                EventBus.Raise(new TableBuilt(_seat));
                EventBus.Raise(new SeatFreed(_seat));
            }
        }
    }
}