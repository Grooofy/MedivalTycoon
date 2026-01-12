using Beers;
using MedivalTycoon;
using Money;
using SeatSyst;
using System.Collections.Generic;
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
        public event UnityAction<Seat> LinedUp;
        public bool IsBuilt => Price <= 0;
        public int Price { get; private set; }

        private Queue<Point> _wayPoints = new Queue<Point>();
        private Seat _seat;
        private BeerTaker _beerTaker;
        private TableInteractionMode _tableInteractionMode;
        private CoinManager _coinManager;
        private SeatInventory _inventory;
        private bool _isBuilding;
        private bool _isTakeEnable;

        private bool _isBeerTakerInitialize;
        private bool _isCoinManagerInitialize;
        private bool _isSeatSystemInitialize;

        public void Initialize(int startPrice, Queue<Point> wayPoint)
        {
            Price = startPrice;
            _wayPoints = wayPoint;
            _seat = GetComponentInChildren<Seat>();
            _beerTaker = GetComponentInChildren<BeerTaker>();
            _inventory = GetComponentInChildren<SeatInventory>();
            _seat.InventoryFulling += SwitchCheckHits;
            _tableInteractionMode = GetComponentInChildren<TableInteractionMode>();   
            _coinManager = GetComponentInChildren<CoinManager>();
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
            if (_beerTaker != null && _tableInteractionMode != null)
            {
                _beerTaker.Initialize(_inventory, _waiterLayer);
                _tableInteractionMode.Initialize();
                _isBeerTakerInitialize = true;
            }
        }

        public void InitializeCoinManager(PropsSpawner propsSpawner)
        {
            if (_coinManager != null && _isBeerTakerInitialize)
            {
                _coinManager.Initialize(propsSpawner);
                _isCoinManagerInitialize = true;
            }
                
        }

        public void InitializeSeatSystem(IPropsPool propsPool)
        {
            if (_seat != null && _beerTaker != null && _isCoinManagerInitialize && _isBeerTakerInitialize)
            {
                _seat.Initialize(_coinManager.GetIProps(), _tableInteractionMode, _wayPoints, _visitorLayer, _inventory, propsPool, _resetDelay);
                _isSeatSystemInitialize = true;
            }
                
        }

        public void CheckHits()
        {
            if (_seat == null && _beerTaker == null && _coinManager == null ) return;

            if (_isBuilding && _isTakeEnable)
            {
                _beerTaker.CheckHits();
            }

            _seat.CheckHits();
            _coinManager.CheckHits();
        }

        public void ReducePrice(int step)
        {
            Price = Mathf.Max(Price - step, 0);
            PriceChanged?.Invoke(Price);

            if (Price <= 0 && !_isBuilding)
            {
                _isBuilding = true;
                _isTakeEnable = true;
                LinedUp?.Invoke(_seat);
            }
        }

        private void OnDestroy()
        {
            _seat.InventoryFulling -= SwitchCheckHits;
        }
    }
}