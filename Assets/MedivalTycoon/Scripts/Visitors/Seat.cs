using UnityEngine;
using UnityEngine.UI;

public class Seat : MonoBehaviour
{
    public bool IsOccupied { get; private set; } // Свободно ли место
    private TavernGuest currentGuest;           // Текущий гость на месте
    private int requiredBeerAmount;             // Необходимое количество пива
    private int deliveredBeerAmount;            // Доставленное количество пива

    [SerializeField] private Text beerText;     // Текстовое поле для отображения количества пива

    public delegate void SeatVacatedHandler(Seat seat);
    public event SeatVacatedHandler OnSeatVacated; // Событие освобождения места

    private void Start()
    {
        IsOccupied = false;
        UpdateBeerDisplay(0); // Инициализация текста
    }

    // Занять место
    public void Occupy(TavernGuest guest)
    {
        if (!IsOccupied)
        {
            currentGuest = guest;
            IsOccupied = true;

            // Установить необходимое количество пива
            requiredBeerAmount = guest.GetBeerAmount();
            deliveredBeerAmount = 0;

            // Обновить отображение пива
            UpdateBeerDisplay(requiredBeerAmount);

            Debug.Log($"Место занято. Необходимо пива: {requiredBeerAmount}");
        }
    }

    // Освободить место
    public void Vacate()
    {
        if (IsOccupied)
        {
            currentGuest = null;
            IsOccupied = false;

            // Сбросить отображение пива
            UpdateBeerDisplay(0);

            Debug.Log("Место освобождено.");

            // Вызываем событие освобождения места
            OnSeatVacated?.Invoke(this);
        }
    }

    // Доставка пива на место
    public void DeliverBeer(int amount)
    {
        if (IsOccupied && deliveredBeerAmount < requiredBeerAmount)
        {
            deliveredBeerAmount += amount;
            deliveredBeerAmount = Mathf.Min(deliveredBeerAmount, requiredBeerAmount);

            // Обновить отображение пива
            UpdateBeerDisplay(requiredBeerAmount - deliveredBeerAmount);

            Debug.Log($"Доставлено пива: {deliveredBeerAmount}/{requiredBeerAmount}");

            // Если заказ выполнен
            if (deliveredBeerAmount >= requiredBeerAmount)
            {
                Debug.Log("Заказ выполнен!");
                currentGuest.OrderCompleted();
            }
        }
    }

    // Обновить отображение пива
    private void UpdateBeerDisplay(int remainingBeer)
    {
        if (beerText != null)
        {
            beerText.text = $"Пива: {remainingBeer}";
        }
    }
}