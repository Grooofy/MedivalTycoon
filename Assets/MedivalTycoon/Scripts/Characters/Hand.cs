using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    private Queue<Props> _propses = new Queue<Props>();

    private int _indexPoint = 0;

    public void TakeObject(Props barrel)
    {
        barrel.TryMoveTo(_points[_indexPoint].transform);
    }
   

   
}
