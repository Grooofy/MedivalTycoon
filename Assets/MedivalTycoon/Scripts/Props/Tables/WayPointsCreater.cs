using Characters;
using System.Collections.Generic;
using Tables;
using UnityEngine;


public class WayPointsCreater : MonoBehaviour
{
    [SerializeField] private Point _startWayPoint;
    
    private Queue<Point> _way;

    private Vector3 _step = new Vector3(0, 0, 1f);


    public Queue<Point> CreatePoints(GridCell cell, int maxSteps = 3)
    {
        _way = new Queue<Point>();

        int steps = Mathf.Min(cell.z, maxSteps);

        for (int i = 0; i <= steps; i++)
        {
            var point = ObjectFactory.CreateObjectWithComponent<Point>($"Point {i}");
            point.transform.SetParent(_startWayPoint.transform);
            point.transform.position = _startWayPoint.transform.position + _step * i;
            _way.Enqueue(point);
        }
        return _way;
    }
}
