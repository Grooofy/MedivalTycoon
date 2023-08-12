using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private PropseMover _mover;
    public int MaxNumberProps => _points.Count;

    private Queue<GameObject> _items = new Queue<GameObject>();
    private int _minNumberProps = 0;
    private GameObject _props;

   

    private void OnEnable()
    {
        _mover.MovedEnded += GoNextPoint;
    }

    private void OnDisable()
    {
        _mover.MovedEnded -= GoNextPoint;
    }

    public void TryTakeObject(List<GameObject> props)
    {
        if (_minNumberProps == MaxNumberProps)
        {
            return;
        }
        if (props != null)
        {
            foreach (var prop in props)
            {
                TakeObject(prop);
            }
        }
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
