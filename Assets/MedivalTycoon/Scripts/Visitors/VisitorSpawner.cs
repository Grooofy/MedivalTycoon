using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisitorSpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private Visitor _visitorPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private int _maxVisitors = 10;
    [SerializeField] private float _queueSpacing = 1.5f;
    [SerializeField] private bool _spawnRightToLeft = true; // Новый параметр направления

    private Queue<Visitor> _waitingQueue = new Queue<Visitor>();
    private List<Visitor> _allVisitors = new List<Visitor>();

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
        SeatManager.Instance.OnSeatAvailable += OnSeatAvailable;
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            
            if (_allVisitors.Count < _maxVisitors)
            {
                SpawnVisitor();
            }
        }
    }

    private void SpawnVisitor()
    {
        Transform spawnPoint = GetSpawnPoint();
        Visitor visitor = Instantiate(_visitorPrefab, spawnPoint.position, Quaternion.identity);
        visitor.Initialize(Random.Range(1, 4));
        _allVisitors.Add(visitor);
        
        AddToQueue(visitor);
        TryAssignSeat(visitor);
    }

    private Transform GetSpawnPoint()
    {
        // Если направление справа-налево, берем последнюю точку спавна
        int index = _spawnRightToLeft ? 
            _spawnPoints.Length - 1 : 
            Random.Range(0, _spawnPoints.Length);
        
        return _spawnPoints[index];
    }

    private void AddToQueue(Visitor visitor)
    {
        _waitingQueue.Enqueue(visitor);
        UpdateQueuePositions();
    }

    private void UpdateQueuePositions()
    {
        int index = 0;
        int directionModifier = _spawnRightToLeft ? -1 : 1;
        
        foreach (Visitor visitor in _waitingQueue)
        {
            Vector3 position = transform.position + 
                new Vector3(index * _queueSpacing * directionModifier, 0, 0);
            
            visitor.MoveToPosition(position);
            index++;
        }
    }

    private void TryAssignSeat(Visitor visitor)
    {
        Seat seat = SeatManager.Instance.GetAvailableSeat();
        if (seat != null)
        {
            _waitingQueue.Dequeue();
            visitor.AssignSeat(seat);
            UpdateQueuePositions();
        }
    }

    private void OnSeatAvailable()
    {
        if (_waitingQueue.Count > 0)
        {
            TryAssignSeat(_waitingQueue.Peek());
        }
    }

    // Метод для изменения направления в runtime
    public void ChangeSpawnDirection(bool rightToLeft)
    {
        _spawnRightToLeft = rightToLeft;
        UpdateQueuePositions();
    }

    private void OnDestroy()
    {
        if (SeatManager.Instance != null)
            SeatManager.Instance.OnSeatAvailable -= OnSeatAvailable;
    }
}