using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private PropseMover _mover;

    private Queue<GameObject> _items = new Queue<GameObject>();
    private int _maxNumberProps => _points.Count;
    private int _minNumberProps = 0;
    private GameObject _props;


    private void OnEnable()
    {
        _mover.MovementOver += GoNextPoint;
    }

    private void OnDisable()
    {
        _mover.MovementOver -= GoNextPoint;
    }

    public void TryTakeObject(GameObject props)
    {
        if (_minNumberProps == _maxNumberProps)
        {
            return;
        }
        if (props != null)
        {
            TakeObject(props);
        }
        return;
    }

    private void TakeObject(GameObject props)
    {
        _props = props;
        _mover.MoveThroughOnePoints(_points[_minNumberProps], props);
    } 

    private void GoNextPoint()
    {
        _items.Enqueue(_props);
        _minNumberProps++;
    }
}
