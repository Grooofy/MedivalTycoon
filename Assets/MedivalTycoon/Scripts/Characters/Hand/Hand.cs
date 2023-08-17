using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private HandPoints _point;
    public bool IsTaked;

    private Transform _curentPoint;
    private IProps _props;


    private void Update()
    {
        if (IsTaked)
        {
            _props.Move(_curentPoint.position);
            _props.SetNewParent(transform);
        }
    }

    public void Take(IProps props)
    {
        _curentPoint = _point.GetEmptyPoint();
        
        if (_curentPoint == null)
            return;
        _props = props;
    }
}
