using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeerCreator : MonoBehaviour, IPropsMover
{
    public Action<bool> Fulling;
    [SerializeField] private Point _barelPoint;
    [SerializeField] private List<Point> _points = new List<Point>();
    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointsProps = new Queue<Props>();
    private WaitForSeconds _wait = new WaitForSeconds(0.3f);
    private Props _currentProps;
    private int _index;
    private bool _isFull;


    private void OnEnable()
    {
        _barelPoint.Filling += MoveBearToPoint;
    }


    private void MoveBearToPoint(bool value)
    {
        StartCoroutine(FillingPoints());
    }

    public void RegisterProps(Queue<Props> props)
    {
        if (props == null)
        {
            Debug.LogError("Null register props!");
            return;
        }

        if (props.Count == 0)
        {
            Debug.LogWarning("Empty props queue passed to RegisterProps.");
            return;
        }

        foreach (var prop in props)
        {
            if (prop == null)
            {
                Debug.LogWarning("Null prop found in the queue. Skipping.");
                continue; 
            }

            _props.Enqueue(prop);
        }

        Debug.Log($"Successfully registered {props.Count} props in Regulating.");
    }

    public IEnumerator FillingPoints()
    {
        if (_props.Count == 0) yield break;

        var temporaryQueue = new Queue<Props>();

        while (_isFull == false && _index < _points.Count)
        {
            if (_props.Count == 0) yield break;

            var prop = _props.Peek();
            if (prop == null) yield break;
            
            StartCoroutine(prop.TryMoveTo(_points[_index]));
            temporaryQueue.Enqueue(_props.Dequeue());
            _index++;
            
            if (_index == _points.Count)
            {
                _index = _points.Count - 1;
                Fulling?.Invoke(true);
                _isFull = true;
                _pointsProps = new Queue<Props>(temporaryQueue.Reverse());
            }
            _pointsProps = new Queue<Props>(temporaryQueue.Reverse());
            yield return _wait;
        }
    }

    public Queue<Props> GetTo(int amount)
    {
        throw new System.NotImplementedException();
    }
}
