using System.Collections;
using UnityEngine;

public class TavernGuest : MonoBehaviour
{
    // Состояния гостя
    public enum GuestState { InQueue, MovingToSeat, WaitingForOrder, Satisfied, Leaving }
    private GuestState currentState;

    // Параметры гостя
    [SerializeField] private float minWaitTime = 10f; // Минимальное время ожидания заказа
    [SerializeField] private float maxWaitTime = 20f; // Максимальное время ожидания заказа
    private int beerAmount;                          // Количество пива, которое гость хочет заказать
    private float waitTimer;                         // Таймер ожидания
    private float moveSpeed = 2f;                    // Скорость перемещения

    // Цели для перемещения
    private Transform targetPosition;               // Цель для перемещения
    private Transform exitPosition;                 // Выход из таверны
    private Transform queueTargetPosition;

    // Компоненты
    private Seat currentSeat;                       // Текущее место гостя
    private bool isInteractable = false;            // Флаг для взаимодействия с охранником

    private void Start()
    {
        // Инициализация
        currentState = GuestState.InQueue;
        beerAmount = Random.Range(1, 5); // Случайное количество пива
        StartCoroutine(GuestBehavior());
    }

    private IEnumerator GuestBehavior()
    {
        while (true)
        {
            switch (currentState)
            {
                case GuestState.InQueue:
                    // Гость ждёт в очереди
                    yield return null;
                    break;

                case GuestState.MovingToSeat:
                    // Гость движется к месту
                    MoveToTarget();
                    yield return null;
                    break;

                case GuestState.WaitingForOrder:
                    // Гость ждёт выполнения заказа
                    WaitOrder();
                    yield return null;
                    break;

                case GuestState.Satisfied:
                    // Гость доволен и остаётся на месте
                    Debug.Log("Гость доволен и ждёт выноса.");
                    isInteractable = true; // Гость становится доступным для переноски
                    yield break;

                case GuestState.Leaving:
                    // Гость покидает таверну
                    LeaveTavern();
                    yield break;
            }

            yield return null;
        }
    }

    public void MoveToQueuePosition(Transform targetPosition)
    {
        queueTargetPosition = targetPosition;
        StartCoroutine(MoveToQueueTarget());
    }

    private IEnumerator MoveToQueueTarget()
    {
        while (queueTargetPosition != null)
        {
            // Плавное перемещение к цели
            transform.position = Vector3.MoveTowards(transform.position, queueTargetPosition.position, moveSpeed * Time.deltaTime);

            // Проверяем, достиг ли гость цели
            if (Vector3.Distance(transform.position, queueTargetPosition.position) < 0.1f)
            {
                Debug.Log("Гость достиг своей позиции в очереди.");
                queueTargetPosition = null; // Очищаем цель
            }

            yield return null;
        }
    }
    
    public void AssignSeat(Seat seat)
    {
        if (seat != null && currentState == GuestState.InQueue)
        {
            currentSeat = seat;
            currentState = GuestState.MovingToSeat;
            targetPosition = seat.transform; // Устанавливаем цель для перемещения
            Debug.Log($"Гость начинает движение к месту. Заказ: {beerAmount} кружек пива.");
        }
    }

    private void MoveToTarget()
    {
        if (targetPosition != null)
        {
            // Плавное перемещение к цели
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Проверяем, достиг ли гость цели
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                Debug.Log("Гость достиг места.");
                currentState = GuestState.WaitingForOrder;
                targetPosition = null; // Очищаем цель
                waitTimer = 0f; // Сбрасываем таймер
            }
        }
    }

    private void WaitOrder()
    {
        waitTimer += Time.deltaTime;

        if (waitTimer >= Random.Range(minWaitTime, maxWaitTime))
        {
            Debug.Log("Заказ не выполнен вовремя. Гость уходит.");
            currentState = GuestState.Leaving;
            currentSeat.Vacate();
        }
    }

    public void DeliverBeer(int amount)
    {
        if (currentState == GuestState.WaitingForOrder)
        {
            beerAmount -= amount;
            Debug.Log($"Доставлено пива: {amount}. Осталось: {beerAmount}");

            if (beerAmount <= 0)
            {
                Debug.Log("Заказ выполнен!");
                currentState = GuestState.Satisfied;
            }
        }
    }

    public void InteractWithGuard(Transform exit)
    {
        if (isInteractable && currentState == GuestState.Satisfied)
        {
            Debug.Log("Охранник взаимодействует с гостем.");
            currentState = GuestState.Leaving;
            exitPosition = exit; // Устанавливаем выход
            targetPosition = exitPosition; // Начинаем движение к выходу
        }
    }

    private void LeaveTavern()
    {
        if (targetPosition != null)
        {
            // Плавное перемещение к выходу
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Проверяем, достиг ли гость выхода
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                Debug.Log("Гость покинул таверну.");
                Destroy(gameObject); // Уничтожаем объект гостя
            }
        }
    }

    public void OrderCompleted()
    {
        throw new System.NotImplementedException();
    }

    public int GetBeerAmount()
    {
        return beerAmount;
    }
}