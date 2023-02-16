using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _step;
    private float _offset;
    private Queue<Prop> _myProps = new Queue<Prop>();
    private int _maxCountProps = 5;
 
    
    public void  TryRaiseObject(Prop prop)
    {
        if (_myProps.Count == _maxCountProps)
            return;
        
        Vector3 firstPoint = CalculateNewPosition(_point.position);
        prop.ChangeOwner(this.gameObject, _myProps.Count < 1 ? firstPoint : CalculateNewPosition(firstPoint));
        _myProps.Enqueue(prop);
        CalculateOffSet();
    }

    private Vector3 CalculateNewPosition(Vector3 point)
    {
        Vector3 position = new Vector3(point.x, point.y + _offset, point.z);
        return position;
    }

    private void CalculateOffSet()
    {
        _offset += _step;
    }
}
