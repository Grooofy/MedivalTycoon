using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hand : MonoBehaviour
{
    public Action<bool> HandFulling;
    [SerializeField] private List<Point> _pointsProps;

    private Queue<Props> _props = new Queue<Props>();
    private Queue<Props> _pointProps = new Queue<Props>();
    private WaitForSeconds _wait = new WaitForSeconds(0.2f);
    private Props _currentProps;
    private int _index = 0;
    private bool _isFulling;


    public void RegisterProps(Regulating regulating)
    {
        if (_isFulling) return;

        if (regulating == null) return;
        
        if (regulating.GetTo(_pointsProps.Count) == null) return;

        _props = regulating.GetTo(_pointsProps.Count);
    }


    public IEnumerator FillingPoints()
    {
        while (_props.Count > 0)
        {
            StartCoroutine(_props.Peek().TryMoveTo(_pointsProps[_index]));
            _pointProps.Enqueue(_props.Dequeue());
            _index++;
            Debug.Log(_index);
            if (_index == _pointsProps.Count)
            {
                _isFulling = true;
                HandFulling?.Invoke(true);
            }

            yield return _wait;
        }
    }

    public Props GetOne()
    {
        if (_pointsProps.Count == 0) return null;

        return _pointProps.Dequeue();
    }
}