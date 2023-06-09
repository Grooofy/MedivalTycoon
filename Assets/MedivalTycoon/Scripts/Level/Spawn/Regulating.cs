using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Regulating : MonoBehaviour
{
    [SerializeField] private List<Point> _points;
    [SerializeField] private PropseMover _mover;

    public int CountBarrel => _points.Count;
    public UnityAction BarrelArrived;

    private Queue<GameObject> _barrels = new Queue<GameObject>();
    private int _pointNumber = 0;
    
    

    private void OnEnable()
    {
        _mover.MovementOver += CalculatePoint;
    }

    private void OnDisable()
    {
        _mover.MovementOver -= CalculatePoint;
    }

    public void MoveObject(GameObject barrel)
    {
        _mover.MoveThroughTwoPoints(_points[_pointNumber].transform.position, barrel);
        _barrels.Enqueue(barrel);
    }  
    
    public GameObject GetObject()
    {
        return _barrels.Dequeue();
    }

    private void CalculatePoint()
    {
        if (_pointNumber == _points.Count - 1)
        {
          return;
        }
        _pointNumber++;
        BarrelArrived?.Invoke();
    }
}