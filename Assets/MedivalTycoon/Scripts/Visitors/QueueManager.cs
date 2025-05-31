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
            if (pos == null)
            {
                Debug.LogError("Queue position not set!");
                continue;
            }
            AddGuestToQueue();
        }
    }

    public void AddGuestToQueue()
    {
        if (guestQueue.Count >= maxQueueLength)
        {
            Debug.LogWarning("Max queue length reached.");
            return;
        }

        var guest = Instantiate(_guest.gameObject, spawnPoint.position, Quaternion.identity).GetComponent<TavernGuest>();
        guestQueue.Enqueue(guest);
        Debug.Log(guestQueue.Count + " Guest added to queue");
        UpdateQueuePositions();
    }

    public void AssignSeatToNextGuest(Seat seat)
    {
            
        if (guestQueue.Count > 0 && seat != null && !seat.IsOccupied)
        {
            seat.Occupy(guestQueue.Peek());
            var guest = guestQueue.Dequeue();
            guest.AssignSeat(seat); // 👈 Запускает движение
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