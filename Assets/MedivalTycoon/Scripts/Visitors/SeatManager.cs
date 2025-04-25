using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public static SeatManager Instance { get; private set; }
    public event System.Action OnSeatAvailable;

    private Queue<Seat> _availableSeats = new Queue<Seat>();

    private void Awake() => Instance = this;

    public void RegisterSeat(Seat seat)
    {
        _availableSeats.Enqueue(seat);
        OnSeatAvailable?.Invoke();
    }

    public Seat GetAvailableSeat() => 
        _availableSeats.Count > 0 ? _availableSeats.Dequeue() : null;

    public void ReturnSeat(Seat seat)
    {
        seat.Release();
        _availableSeats.Enqueue(seat);
        OnSeatAvailable?.Invoke();
    }
}