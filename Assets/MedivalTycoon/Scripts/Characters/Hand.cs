using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Hand : MonoBehaviour, IPropsMover
{
    public Action<bool> Fulling { get; set; }
    [SerializeField] private List<Point> _points;
    [SerializeField] public bool IsFull;

    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _handProps = new Queue<Props>(); 
    private WaitForSeconds _wait = new WaitForSeconds(0.2f);
    private Props _currentProps;
    private int _index;


    public void RegisterProps(IPropsMover regulating)
    {
        if (regulating == null)
        {
            return;
        }
        _props = regulating.GetTo(_points.Count);
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
                Fulling?.Invoke(true);
                _handProps = new Queue<Props>(temporaryQueue.Reverse());
            }
            yield return _wait;
        }
    }

    public Queue<Props> GetTo(int amount)
    {
        if (_handProps.Count == 0) return new Queue<Props>();

        if (amount > _handProps.Count)
        {
            amount = _handProps.Count;
            _index = amount;
        }

        var queue = new Queue<Props>();

        for (int i = 0; i < amount; i++)
        {
            queue.Enqueue(_handProps.Dequeue());
            _points[_index -1].Free();
            _index--;
            IsFull = false;
            if (_index < 0) _index = 0; 
        }
        return queue;
    }
    public void RegisterProps(Queue<Props> props) { }

    public void RegisterProp(Props props) { }

}
