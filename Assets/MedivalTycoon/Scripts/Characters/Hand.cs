using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private PropseMover _mover;

    private int _maxNumberProps => _points.Count;
    private int _numberProps = 0;

    public void TakeObject(GameObject props)
    {
        _mover.MoveToPoint(_points[_numberProps].position, props, true);
        props.transform.SetParent(transform);
        _numberProps++;
    } 

}
