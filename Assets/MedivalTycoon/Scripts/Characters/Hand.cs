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
    [FormerlySerializedAs("_isFull")] [HideInInspector] public bool IsFull;

    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointProps = new Queue<Props>(); 
    private WaitForSeconds _wait = new WaitForSeconds(0.2f);
    private Props _currentProps;
    private int _index;


    public void RegisterProps(Regulating regulating)
    {
        if (regulating == null) return;
        
        _props = regulating.GetTo(_points.Count);
        _index = 0;
    }


    public IEnumerator FillingPoints()
    {
        var temporaryQueue = new Queue<Props>();
        
        while (IsFull == false && _index < _points.Count)
        {
            StartCoroutine(_props.Peek().TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;
            
            if (_index == _points.Count)
            {
                _index = _points.Count - 1;
                IsFull = true;
                HandFulling?.Invoke(true);
                _pointProps = new Queue<Props>(temporaryQueue.Reverse());
            }

            yield return _wait;
        }
    }

    public Queue<Props> GetTo()
    {
        if (_pointProps.Count == 0) return null;
        if(_index <= 0) return null;

        var queue = new Queue<Props>();
        
        for (int i = 0 ; i < _points.Count; i++)
        {
            queue.Enqueue(_pointProps.Dequeue());
            _points[_index].IsFill = false;
            _index--;
            IsFull = false;
        }
        return queue;
    }

    private void Update()
    {
        Debug.Log(_index);
    }
}