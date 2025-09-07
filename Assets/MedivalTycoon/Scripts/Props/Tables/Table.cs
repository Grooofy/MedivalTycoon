using Beers;
using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Tables
{
    public class Table : MonoBehaviour
    {
        public event UnityAction<int> PriceChanged;
        public event UnityAction LinedUp;
        public bool IsBuilt => Price <= 0;
        public int Price { get; private set; }
        
        private Seat _seat;
        private BeerTaker _beerTaker;


        public void Initialize(int startPrice)
        {
            Price = startPrice;
            _seat = GetComponentInChildren<Seat>();
            _beerTaker = GetComponentInChildren<BeerTaker>();
        }

        public void ReducePrice(int step)
        {
            Price = Mathf.Max(Price - step, 0);
            PriceChanged?.Invoke(Price);

            if (Price <= 0)
            {
                LinedUp?.Invoke();
                EventBus.Raise(new TableBuilt(_seat));
                EventBus.Raise(new SeatFreed(_seat));
            }
        }
    }
}