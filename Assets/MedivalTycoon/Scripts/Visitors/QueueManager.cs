using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [SerializeField] private TavernGuest _guest;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] queuePositions;
    [SerializeField] private int maxQueueLength = 5;
    [SerializeField] private SeatManager _seatManager;

    private Queue<TavernGuest> guestQueue = new Queue<TavernGuest>();

    private void Start()
    {
        foreach (var pos in queuePositions)
        {
            if (pos == null) Debug.LogError("Queue position not set!");
            AddGuestToQueue();
        }
    }

    public void AddGuestToQueue()
    {
        if (guestQueue.Count >= maxQueueLength) return;

        var guest = Instantiate(_guest.gameObject, spawnPoint.position, Quaternion.identity).GetComponent<TavernGuest>();
        guestQueue.Enqueue(guest);
        Debug.Log(guestQueue.Count + " Guest added to queue");
        UpdateQueuePositions();
    }

    public void AssignSeatToNextGuest(Seat seat)
    {
        Debug.Log(guestQueue.Count + "seat!!!");
        if (guestQueue.Count > 0 && seat != null && !seat.IsOccupied)
        {
            Debug.Log("Assigning seat to guest");
            var guest = guestQueue.Dequeue();
            seat.Occupy(guest); // 🚨 Блокируем место сразу
            guest.AssignSeat(seat); // Гость начинает движение
            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach (var guest in guestQueue)
        {
            if (index < queuePositions.Length)
                guest.MoveToQueuePosition(queuePositions[index]);
            index++;
        }
    }
}