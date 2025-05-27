using TMPro;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beerText;
    public bool IsOccupied { get; private set; }
    private TavernGuest guest;
    private int requiredBeer;
    private int deliveredBeer;

    public delegate void SeatVacatedHandler();
    public event SeatVacatedHandler OnSeatVacated;

    public void Occupy(TavernGuest guest)
    {
        if (!IsOccupied)
        {
            IsOccupied = true;
            this.guest = guest;
            requiredBeer = guest.GetBeerAmount();
            deliveredBeer = 0;
            UpdateBeerDisplay(requiredBeer);
        }
    }

    public void Vacate()
    {
        IsOccupied = false;
        guest = null;
        UpdateBeerDisplay(0);
        OnSeatVacated?.Invoke();
    }

    public void DeliverBeer(int amount)
    {
        deliveredBeer += amount;
        deliveredBeer = Mathf.Min(deliveredBeer, requiredBeer);
        UpdateBeerDisplay(requiredBeer - deliveredBeer);

        if (deliveredBeer >= requiredBeer)
            guest.OrderCompleted();
    }

    private void UpdateBeerDisplay(int remaining)
    {
        beerText.text = $"Beer: {remaining}";
    }
}