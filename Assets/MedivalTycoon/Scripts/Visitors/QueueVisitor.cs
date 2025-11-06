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
    [SerializeField] private LayerMask _visitorsLayer;

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
            var visitor = _visitorsSpawner.CreateVisitor(point.transform, _speed, _maxBeerCount, _maxWaitTime, _exitPoint.GetPosition());
            _guestQueue.Enqueue(visitor);
            _createdPoints.Add(point);
        }
    }

    public void UpdateState()
    {
        _visitorsSpawner.UpdateState();
        _exitPoint.CheckHits();
    }

    private void OnSeatFreed(SeatFreed seatFreed)
    {
        Debug.Log($"[QueueVisitor] Получено событие SeatFreed для места {seatFreed.Seat.name}, Instance ID: {seatFreed.Seat.GetInstanceID()}");

        if (_seatAggregator.FreeSeats.Contains(seatFreed.Seat))
        {
            Debug.Log($"[QueueVisitor] Место {seatFreed.Seat.name} свободно. Отправляем гостя.");
            TryAssignSpecificSeat(seatFreed.Seat);
        }
        else
        {
            Debug.LogWarning($"[QueueVisitor] Место {seatFreed.Seat.name} уже занято. Пропускаем.");
        }

        MoveQueue();
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
        // Проверяем, что место действительно в списке свободных у SeatAggregator
        if (!_seatAggregator.FreeSeats.Contains(seat))
        {
            Debug.LogWarning($"[QueueVisitor] Место {seat.name} больше не свободно. Пропускаем.");
            return;
        }

        if (_guestQueue.Count == 0)
        {
            Debug.Log("[QueueVisitor] Очередь пуста, никто не садится.");
            return;
        }

        if (_guestQueue.TryDequeue(out var visitor))
        {
            Vector3 targetPosition = seat.GetPosition(); // <- Проверь эту позицию
            Debug.Log($"[QueueVisitor] Отправляем {visitor.name} к месту {seat.name} на позицию {targetPosition}");

            visitor.GoTo(targetPosition); // <- Проверь, что GoTo корректно обрабатывает это

            EventBus.Raise(new SeatTaken(seat));
        }
        else
        {
            Debug.Log("[QueueVisitor] Не удалось извлечь посетителя из очереди.");
        }
    }



    private void OnDestroy()
    {
        _seatAggregator.OnDestroy();
        EventBus.Unsubscribe<SeatFreed>(OnSeatFreed);
        EventBus.Unsubscribe<TableBuilt>(OnTableBuilt);
    }

}