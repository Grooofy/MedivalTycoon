using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Visitor _visitorPrefab;
    [SerializeField] private int _initialVisitors = 5;
    [SerializeField] private Transform _queueStart;
    [SerializeField] private float _queueSpacing = 1f;

    private Queue<Visitor> _visitorsQueue = new Queue<Visitor>();
    private List<Seat> _availableSeats = new List<Seat>();
    public int QueueCount => _visitorsQueue.Count;

    private void Start()
    {
        InitializeVisitors();
    }

    private void InitializeVisitors()
    {
        for(int i = 0; i < _initialVisitors; i++)
        {
            CreateVisitor();
        }
        UpdateQueuePositions();
    }

    private void CreateVisitor()
    {
        Visitor visitor = Instantiate(_visitorPrefab);
        visitor.Initialize(
            Random.Range(1, 4),    // Количество заказов
            Random.Range(5f, 10f)  // Время приготовления
        );
        _visitorsQueue.Enqueue(visitor);
    }

    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach(Visitor visitor in _visitorsQueue)
        {
            Vector3 target = _queueStart.position + 
                             new Vector3(-index * _queueSpacing, 0, 0);
            visitor.MoveToSeat(target);
            index++;
        }
    }

    // Вызывается при добавлении нового стола
    public void AddSeats(List<Seat> newSeats)
    {
        _availableSeats.AddRange(newSeats);
        AssignVisitorsToSeats();
    }

    public IEnumerable<Visitor> GetAllVisitors()
    {
        return _visitorsQueue.ToArray();
    }

    public void AddVisitorToQueue(Visitor visitor)
    {
        _visitorsQueue.Enqueue(visitor);
        UpdateQueuePositions();
        AssignVisitorsToSeats();
    }
    
    private void AssignVisitorsToSeats()
    {
        foreach(Seat seat in _availableSeats)
        {
            if(!seat.IsOccupied && _visitorsQueue.Count > 0)
            {
                Visitor visitor = _visitorsQueue.Dequeue();
                seat.AssignVisitor(visitor);
            }
        }
        UpdateQueuePositions();
    }

    // Вызывается при освобождении места
    public void OnSeatReleased(Seat seat)
    {
        seat.ReleaseSeat();
        AssignVisitorsToSeats();
    }
}