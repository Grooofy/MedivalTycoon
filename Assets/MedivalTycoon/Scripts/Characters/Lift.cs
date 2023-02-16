using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _step;
    private Vector3 _newPosition;
    private float _offset;
    private Queue<Prop> _myProps = new Queue<Prop>();
    private int _maxCoutProps = 5;

    private void Awake()
    {
        _newPosition = _point.position;
    }

    public void  TryRaiseObject(Prop prop)
    {
        if (_myProps.Count == _maxCoutProps)
            return;

        prop.ChangeOwner(this.gameObject, _newPosition);
        _myProps.Enqueue(prop);
        _newPosition = CalculateNewPosition();
    }

    private Vector3 CalculateNewPosition()
    {
        Vector3 position = new Vector3(_point.position.x, _point.position.y + CalculateOffSet(_offset), _point.position.z);
        return position;
    }

    private float CalculateOffSet(float offset)
    {
        offset += _step;
        return offset;
    }
}
