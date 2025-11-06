using System.Collections.Generic;
using Events;
using System.Linq;
using UnityEngine;

public class SeatAggregator
{
    private static SeatAggregator _instance;
    public static SeatAggregator Instance => _instance ??= new SeatAggregator();

    private List<Seat> _freeSeats = new List<Seat>();
    private List<Seat> _occupiedSeats = new List<Seat>();

    public IReadOnlyList<Seat> FreeSeats => _freeSeats.AsReadOnly();
    public IReadOnlyList<Seat> OccupiedSeats => _occupiedSeats.AsReadOnly();

    public SeatAggregator()
    {
        EventBus.Subscribe<TableBuilt>(OnTableBuilt);
        EventBus.Subscribe<SeatFreed>(OnSeatFreed);
        EventBus.Subscribe<SeatTaken>(OnSeatTaken);
    }

    private void OnTableBuilt(TableBuilt tableEvent)
    {
        var seat = tableEvent.SeatPoint;


        if (seat != null && !_freeSeats.Contains(seat) && !_occupiedSeats.Contains(seat))
        {
            _freeSeats.Add(seat);
        }

    }

    private void OnSeatTaken(SeatTaken seatEvent)
    {
        MoveToOccupied(seatEvent.Seat);
    }


    private void OnSeatFreed(SeatFreed seatEvent)
    {
        MoveToFree(seatEvent.Seat);
    }

    public bool TryGetFreeSeat(out Seat seat)
    {
        seat = _freeSeats.FirstOrDefault();
        return seat != null;
    }

    public List<Seat> GetAvailableSeats(int count)
    {
        return _freeSeats.Take(count).ToList();
    }

    private void MoveToOccupied(Seat seat)
    {
        if (seat == null) return;

        _freeSeats.Remove(seat);

        _occupiedSeats.Add(seat);

    }

    private void MoveToFree(Seat seat)
    {
        if (seat == null) return;

        _occupiedSeats.Remove(seat);

        _freeSeats.Add(seat);

    }
    public void RemoveSeat(Seat seat)
    {
        _freeSeats.Remove(seat);
        _occupiedSeats.Remove(seat);
    }

    public void OnDestroy()
    {
        EventBus.Unsubscribe<TableBuilt>(OnTableBuilt);
        EventBus.Unsubscribe<SeatTaken>(OnSeatTaken);
        EventBus.Unsubscribe<SeatFreed>(OnSeatFreed);
    }
}