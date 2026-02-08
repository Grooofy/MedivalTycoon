using Characters;
using Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Visitors;

public class QueueVisitor : MonoBehaviour
{
    [SerializeField] private VisitorsSpawner _visitorsSpawner;
    [SerializeField] private ExitPoint _exitPoint;
    [SerializeField] private SleepVisitorsTaker _sleepVisitorsTaker;
    [SerializeField] private LayerMask _visitorsLayer;
    [SerializeField] private LayerMask _securityLayer;

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
    private TavernVisitor _currentVisitor;



    public void Initialize(int numberOfObjects, float spacing, float speed, int maxBeerCount, float maxWaitTime)
    {
        _seatAggregator = SeatAggregator.Instance;
        _numberOfObjects = numberOfObjects;
        _spacing = spacing;
        _speed = speed;
        _maxWaitTime = maxWaitTime;
        _maxBeerCount = maxBeerCount;
        _exitPoint.Initialize(_visitorsLayer);
        _sleepVisitorsTaker.Initialize(_exitPoint, _securityLayer);
        _isInitialized = true;
        EventBus.Subscribe<SeatFreed>(OnSeatFreed);
        EventBus.Subscribe<TableBuilt>(OnTableBuilt);
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
            var visitor = _visitorsSpawner.CreateVisitor(point.transform, _speed, _maxBeerCount, _maxWaitTime, _exitPoint.GetPosition(), _securityLayer);
            _guestQueue.Enqueue(visitor);
            _createdPoints.Add(point);
        }
    }

    public void UpdateState()
    {
        _visitorsSpawner.UpdateState();
        _exitPoint.CheckHits();
        _sleepVisitorsTaker.CheckHits();
    }

    private void OnSeatFreed(SeatFreed seatFreed)
    {
        if (_seatAggregator.FreeSeats.Contains(seatFreed.Seat))
        {
            TryAssignSpecificSeat(seatFreed.Seat);
        }  
    }

    private void OnTableBuilt(TableBuilt tableBuilt)
    {
        TryAssignSpecificSeat(tableBuilt.SeatPoint);
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
            if (visitor.GetState() == StateEvent.Move) break;

            visitor.transform.position = Vector3.MoveTowards(
                visitor.transform.position,
                targetPosition,
                _speed * Time.deltaTime
            );
            yield return null;
        }

        visitor.transform.position = targetPosition;
    }

    private void TryAssignSpecificSeat(Seat seat)
    {        
        if (!_seatAggregator.FreeSeats.Contains(seat))        
            return;

        if (_guestQueue.Count == 0)
            return;
        

        if (_guestQueue.TryDequeue(out var visitor))
        {
            Queue<Vector3> targetPosition = seat.GetWay();
            visitor.GoTo(targetPosition);            
            MoveQueue();
            EventBus.Raise(new SeatTaken(seat));
        }
       
    }



    private void OnDestroy()
    {
        _seatAggregator.OnDestroy();
        EventBus.Unsubscribe<SeatFreed>(OnSeatFreed);
        EventBus.Unsubscribe<TableBuilt>(OnTableBuilt);
    }

}