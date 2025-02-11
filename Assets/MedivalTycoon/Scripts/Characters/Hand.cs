using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Hand : MonoBehaviour
{
    public Action<bool> HandFulling;
    [SerializeField] private List<Point> _points;
    [SerializeField] public bool IsFull;

    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _handProps = new Queue<Props>(); 
    private WaitForSeconds _wait = new WaitForSeconds(0.2f);
    private Props _currentProps;
    private int _index;


    public void RegisterProps(Regulating regulating)
    {
        if (regulating == null)
        {
            Debug.Log("Error: Null regulating");
            return;
        }
        _props = regulating.GetTo(_points.Count);
        Debug.Log(_props.Count + " длина полученых");
        _index = 0;
    }


    public IEnumerator FillingPoints()
    {
        if(_props.Count == 0) yield break;
        var temporaryQueue = new Queue<Props>();
        
        while (IsFull == false && _index < _points.Count)
        {
            if(_props.Count == 0) yield break;
            
            var prop = _props.Peek();
            if (prop == null) yield break;
            
            StartCoroutine(prop.TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;
            
            if (_props.Count == 0)
            {
                //_index = _points.Count - 1;
                IsFull = true;
                HandFulling?.Invoke(true);
                _handProps = new Queue<Props>(temporaryQueue.Reverse());
            }
            yield return _wait;
        }
    }

    public Queue<Props> GetTo(int amount)
    {
        if (_handProps.Count == 0)
        {
            Debug.LogWarning("Hand is empty. No props to give.");
            return new Queue<Props>();
        }

        if (amount > _handProps.Count)
        {
            amount = _handProps.Count;
            _index = amount;
        }

        var queue = new Queue<Props>();

        for (int i = 0; i < amount; i++)
        {
            queue.Enqueue(_handProps.Dequeue());
            _points[_index -1].IsFill = false;
            _index--;
            IsFull = false;
            if (_index < 0) _index = 0; 
        }

        Debug.Log($"Transferred {queue.Count} props from Hand to Regulating.");
        return queue;
    }

}