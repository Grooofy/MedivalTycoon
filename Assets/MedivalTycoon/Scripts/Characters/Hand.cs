using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Point> _points;

    private Point _curentPoint;
    private Barrel _curentBarrel;

    public void TakeObject(Barrel barrel)
    {
        _curentBarrel = barrel;
        if (_curentPoint.IsFill == false)
        {
          
            _curentPoint.IsFill = true;
        }
        else
        {
            return;
        }
    }
   
}
