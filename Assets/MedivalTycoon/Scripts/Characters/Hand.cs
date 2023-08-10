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

    public void TryTakeObject(GameObject props)
    {
        if (_minNumberProps == _maxNumberProps)
        {
            return;
        }
        TakeObject(props);
    }

    private void TakeObject(GameObject props)
    {
        _items.Enqueue(props);
        _mover.MoveThroughOnePoints(_points[_minNumberProps], props);
        _minNumberProps++;
    } 
}
