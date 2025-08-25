using System.Collections.Generic;
using Characters;
using Events;
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
        EventBus.Subscribe<SeatPointClear>(MoveTest);
    }

    private void MoveTest(SeatPointClear value)
    {
        Debug.Log("ENTER");
        _guestQueue.Peek().SetFinishPosition(value.Position);
        _guestQueue.Peek().ChangeState(StateEvent.Move);
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
            var visitor = _visitorsManager.CreateVisitor(point.transform, _speed, _maxBeerCount);
            _guestQueue.Enqueue(visitor);
            _createdPoints.Add(point);
        }
    }

    public void UpdateState()
    {
        _visitorsManager.UpdateState();
    }

}