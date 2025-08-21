using System.Collections.Generic;
using Characters;
using UnityEngine;
using Visitors;

public class QueueVisitor : MonoBehaviour
{
    [SerializeField] private VisitorsManager _visitorsManager;
    [SerializeField] private SeatManager _seatManager;
   
    private Queue<TavernVisitor> _guestQueue = new Queue<TavernVisitor>();
    private List<Point> _createdPoints = new List<Point>();
    private int _numberOfObjects;     
    private float _spacing; 
    private float _speed;
    private int _maxBeerCount;
    private Vector3 _lineDirection = Vector3.right;
    private bool _isInitialized;


    public void Initialize(int numberOfObjects, float spacing, float speed, int maxBeerCount)
    {
        _numberOfObjects = numberOfObjects;
        _spacing = spacing;
        _speed = speed;
        _maxBeerCount = maxBeerCount;
        _isInitialized = true;
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
            _visitorsManager.CreateVisitor(point.transform, _speed, _maxBeerCount);
            _createdPoints.Add(point);
        }
    }

    public void UpdateState()
    {
        _visitorsManager.UpdateState();
    }
    
    
    
    
    
    

   /* private void Start()
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

        var guest = Instantiate(_guest.gameObject, spawnPoint.position, Quaternion.identity).GetComponent<TavernVisitor>();
        guestQueue.Enqueue(guest);
        Debug.Log(guestQueue.Count + " Guest added to queue");
       // UpdateQueuePositions();
    }

   /* public void AssignSeatToNextGuest(Seat seat)
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
    }*/
}