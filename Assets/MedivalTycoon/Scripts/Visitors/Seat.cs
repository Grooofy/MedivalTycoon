using System;
using TMPro;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beerText;
    public bool IsOccupied { get; private set; }
    private TavernGuest _guest;
    private int requiredBeer;
    private int deliveredBeer;

    
    public Action<Seat> OnSeatVacated;

    public void Occupy(TavernGuest guest)
    {
        if (IsOccupied == false)
        {
            IsOccupied = true; 
            _guest = guest;
            requiredBeer = guest.GetBeerAmount();
            deliveredBeer = 0;
            UpdateBeerDisplay(requiredBeer);
        }
    }

    public TavernGuest GetGuest()
    {
        return _guest;
    }

    public void Vacate()
    {
        IsOccupied = false;
        _guest = null;
        UpdateBeerDisplay(0);
        OnSeatVacated?.Invoke(this);
    }

    public void DeliverBeer(int amount)
    {
        deliveredBeer += amount;
        deliveredBeer = Mathf.Min(deliveredBeer, requiredBeer);
        UpdateBeerDisplay(requiredBeer - deliveredBeer);
        if (deliveredBeer >= requiredBeer)
            _guest.OrderCompleted();
    }

    private void UpdateBeerDisplay(int remaining)
    {
        beerText.text = $"Пиво: {remaining}";
    }
}