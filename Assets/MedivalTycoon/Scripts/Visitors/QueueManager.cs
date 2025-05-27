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
        UpdateQueuePositions();
    }

    public void AssignSeatToNextGuest()
    {
        var availableSeat = GetFirstAvailableSeat();
        if (availableSeat == null) return;

        if (guestQueue.Count > 0)
        {
            var guest = guestQueue.Dequeue();
            availableSeat.Occupy(guest);
            guest.AssignSeat(availableSeat); // 👈 Здесь гость начинает движение
            UpdateQueuePositions();
        }
    }

    private Seat GetFirstAvailableSeat()
    {
        foreach (var seat in _seatManager.seats)
        {
            if (!seat.IsOccupied)
                return seat;
        }
        return null;
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