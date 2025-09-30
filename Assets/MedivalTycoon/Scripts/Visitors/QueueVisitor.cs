using Characters;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visitors;

public class QueueVisitor : MonoBehaviour
{
    [SerializeField] private VisitorsSpawner _visitorsSpawner;
    [SerializeField] private Transform _exitPoint;
    
    private SeatAggregator _seatAggregator;
    private Queue<TavernVisitor> _guestQueue = new Queue<TavernVisitor>();
    private List<Point> _createdPoints = new List<Point>();
    private int _numberOfObjects;     
    private float _spacing; 
    private float _speed;
    private float _maxWaitTime;
    private int _maxBeerCount;
    private Vector3 _lineDirection = Vector3.right;
    private bool _isInitialized;


    public void Initialize(int numberOfObjects, float spacing, float speed, int maxBeerCount, float maxWaitTime)
    {
        _numberOfObjects = numberOfObjects;
        _spacing = spacing;
        _speed = speed;
        _maxWaitTime = maxWaitTime;
        _maxBeerCount = maxBeerCount;
        _isInitialized = true;
        _seatAggregator =  new SeatAggregator();
        EventBus.Subscribe<SeatFreed>(OnSeatFreed);
    }
    
    public void SpawnVisitorsInLine(Vector3 startPosition)
    {
        if (_isInitialized == false) return;
        
        for (int i = 0; i < _numberOfObjects; i++)
        {
            Vector3 spawnPosition = startPosition + _lineDirection.normalized * (_spacing * i);
            var point = ObjectFactory.CreateObjectWithComponent<Point>($"Point {i}");
            point.transform.SetParent(transform);
            point.transform.position = spawnPosition;
            var visitor = _visitorsSpawner.CreateVisitor(point.transform, _speed, _maxBeerCount, _maxWaitTime, _exitPoint.position);
            _guestQueue.Enqueue(visitor);
            _createdPoints.Add(point);
        }
    }
    
    public void UpdateState()
    {
        _visitorsSpawner.UpdateState();
    }

    private void OnSeatFreed(SeatFreed _)
    {
        TryAssignSeats();
        MoveQueue();
    }

    private void MoveQueue()
    {
        if (_guestQueue.Count == 0) return;

       
        var visitors = new List<TavernVisitor>(_guestQueue);

        for (int i = 0; i < visitors.Count; i++)
        {
            var visitor = visitors[i];
            Vector3 targetPosition = _createdPoints[i].transform.position;
            
            StartCoroutine(MoveToPosition(visitor, targetPosition));
        }
    }

    private IEnumerator MoveToPosition(TavernVisitor visitor, Vector3 targetPosition)
    {
        while (Vector3.Distance(visitor.transform.position, targetPosition) > 0.05f)
        {
            visitor.transform.position = Vector3.MoveTowards(
                visitor.transform.position,
                targetPosition,
                _speed * Time.deltaTime
            );
            yield return null;
        }

        visitor.transform.position = targetPosition;
    }

    private void TryAssignSeats()
    {
        while (_guestQueue.Count > 0 && _seatAggregator.FreeSeats.Count > 0)
        {
            if (_seatAggregator.TryGetFreeSeat(out var seat))
            {
                var visitor = _guestQueue.Dequeue();
                visitor.GoTo(seat.GetPosition());
                EventBus.Raise(new SeatTaken(seat));
            }
        }
    }


    private void OnDestroy()
    {
        EventBus.Unsubscribe<SeatFreed>(OnSeatFreed);
    }

}