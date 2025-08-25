using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Visitors;

public class Seat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beerText;
    public bool IsOccupied  { get; private set; }
    private TavernVisitor _guest;
    private int requiredBeer;
    private WaitForSeconds _delay = new WaitForSeconds(0.5f);
    private int deliveredBeer;

    
    public Action<Seat> OnSeatVacated;
  

    public void Occupy(TavernVisitor guest)
    {
        if (IsOccupied == false && guest != null)
        {
            IsOccupied = true; 
            _guest = guest;
        }
    }

    public void Vacate()
    {
        IsOccupied = false;
        
       
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