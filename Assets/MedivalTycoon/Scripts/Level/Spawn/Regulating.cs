using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;
    [SerializeField] private BarrelSpawner _barrelSpawner;

    private Queue<Barrel> _barrels = new Queue<Barrel>();
    private Queue<Point> _queuePoints = new Queue<Point>();

    private Barrel _currentBarrel;
    private Point _currentPoint;
    private int _amountPoints => _points.Count;


    private void Start()
    {
        CreateQueue();
        StartCoroutine(MoveBarrel());
    }
       
    private void CreateQueue()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            var barrel = _barrelSpawner.GetBarrel();
            barrel.MoveEnded += NextBarrel;
            _barrels.Enqueue(barrel);
            _queuePoints.Enqueue(_points[i]);
        }
    }

    private void NextBarrel()
    {
        if (_queuePoints != null || _currentBarrel != null)
        {
            _currentBarrel = _barrels.Dequeue();
            _currentPoint = _queuePoints.Dequeue();
        }
    }

    private IEnumerator MoveBarrel()
    {
        NextBarrel();
       
        while (_points != null)
        {
            _currentBarrel.Move(_currentPoint.transform.position);
            yield return null;           
        }
        _currentBarrel.MoveEnded -= NextBarrel;
    }
}