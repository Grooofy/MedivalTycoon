using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beerText;
    public bool IsOccupied { get; private set; }
    private TavernGuest _guest;
    private int requiredBeer;
    private WaitForSeconds _delay = new WaitForSeconds(0.5f);
    private int deliveredBeer;

    
    public Action<Seat> OnSeatVacated;
  

    public void Occupy(TavernGuest guest)
    {
        if (IsOccupied == false && guest != null)
        {
            IsOccupied = true; 
            _guest = guest;
            _guest.Waiting += SetActiveText;
            requiredBeer = guest.GetBeerAmount();
        }
    }

    public TavernGuest GetGuest()
    {
        return _guest;
    }

    public void Vacate()
    {
        IsOccupied = false;
        
        if(_guest != null)
            _guest.Waiting -= SetActiveText;
        
        _guest = null;
       // SetActiveText(false);
        OnSeatVacated?.Invoke(this);
    }

    public IEnumerator DeliverBeer(int amount)
    {
        while (deliveredBeer < requiredBeer)
        {
            deliveredBeer += amount;
            var currentAmount = requiredBeer - deliveredBeer;
            //SetActiveText(true, currentAmount);

            if (currentAmount <= 0)
            {
                _guest.OrderCompleted();
                deliveredBeer = 0;
            }
            yield return _delay;
        }
    }

    private void SetActiveText(bool value, int remainingBeer = 0)
    {
        beerText.gameObject.SetActive(value);
        UpdateBeerDisplay(remainingBeer);
    }

    private void UpdateBeerDisplay(int remaining)
    {
        beerText.text = $"Пиво: {remaining}";
    }
}