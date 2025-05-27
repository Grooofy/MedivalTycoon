using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    [SerializeField] private QueueManager queueManager;
    private List<Seat> seats = new List<Seat>();

    public void AddSeat(Seat seat)
    {
        seats.Add(seat);
        seat.OnSeatVacated += () => queueManager.AssignSeatToNextGuest(seat);
        Debug.Log(seat  + "seat assigned");
    }

    public void RemoveSeat(Seat seat)
    {
        seats.Remove(seat);
        seat.OnSeatVacated -= () => queueManager.AssignSeatToNextGuest(seat);
    }
}