using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;

    private Transform _curentPoint;
    private IProps _props;


    private void Update()
    {
        if (_props != null)
        {
            _props.Move(_points[0].position);
            _props.SetNewParent(transform);
            _props = null;
        }
    }
    public void Take(IProps props, Transform point)
    {
        _curentPoint = point;
        _props = props;
    }
}
