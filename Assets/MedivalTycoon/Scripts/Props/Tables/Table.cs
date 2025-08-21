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


        public void Initialize(int startPrice)
        {
            Price = startPrice;
        }

        public void ReducePrice(int step)
        {
            Price = Mathf.Max(Price - step, 0);
            PriceChanged?.Invoke(Price);

            if (Price <= 0)
            {
                LinedUp?.Invoke();
            }
        }
    }
}