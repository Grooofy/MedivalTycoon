using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [SerializeField] private GameObject guestPrefab;
    [SerializeField] private Transform spawnPoint; // Точка спавна гостей
    [SerializeField] private Transform[] queuePositions; // Фиксированные позиции в очереди
    [SerializeField] private int maxQueueLength = 5; // Максимальная длина очереди

    private Queue<TavernGuest> guestQueue = new Queue<TavernGuest>(); // Очередь гостей

    private void Start()
    {
        // Инициализация
        foreach (var position in queuePositions)
        {
            if (position == null)
            {
                Debug.LogError("Одна из позиций очереди не назначена!");
            }
            AddGuestToQueue();
        }
    }

    // Добавить гостя в очередь
    public void AddGuestToQueue()
    {
        if (guestQueue.Count >= maxQueueLength)
        {
            Debug.Log("Очередь переполнена. Гость не добавлен.");
            return;
        }

        // Создаём нового гостя
        
        if (guestPrefab == null)
        {
            Debug.LogError("Префаб гостя не найден!");
            return;
        }

        var guestObject = Instantiate(guestPrefab, spawnPoint.position, Quaternion.identity);
        var guest = guestObject.GetComponent<TavernGuest>();

        if (guest != null)
        {
            guestQueue.Enqueue(guest); // Добавляем гостя в очередь
            Debug.Log("Гость добавлен в очередь.");
            UpdateQueuePositions(); // Обновляем позиции гостей в очереди
        }
    }

    // Назначить гостя на свободное место
    public void AssignSeatToNextGuest(Seat seat)
    {
        if (guestQueue.Count > 0 && seat != null && !seat.IsOccupied)
        {
            var nextGuest = guestQueue.Dequeue(); // Берём первого гостя из очереди
            seat.Occupy(nextGuest);              // Занимаем место
            Debug.Log("Гость назначен на свободное место.");
            UpdateQueuePositions();             // Обновляем позиции гостей в очереди
        }
    }

    // Обновить позиции гостей в очереди
    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach (var guest in guestQueue)
        {
            if (index < queuePositions.Length)
            {
                guest.MoveToQueuePosition(queuePositions[index]); // Перемещаем гостя на новую позицию
            }
            index++;
        }
    }
}