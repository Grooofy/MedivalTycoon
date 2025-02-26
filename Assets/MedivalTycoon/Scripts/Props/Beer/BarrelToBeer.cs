using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelToBeer : MonoBehaviour
{
    [SerializeField] private List<Point> _points;
    [SerializeField] private Point _barrelPoint;
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;
    
    private MoverStoper _moverStoper;
    private Queue<Props> _availableProps = new Queue<Props>();
    private int _counter;
    private void OnEnable()
    {
        foreach (var point in _points)
            point.Filling += TurnObject;
    }

    private void OnDisable()
    {
        foreach (var point in _points)
            point.Filling -= TurnObject;
    }
   
    private void TurnObject(bool value)
    {
        _counter++;
        _animator.SetBool("IsOn", !value);
        _collider.enabled = value;
        _uiObject.SetActive(value);
        
        if (_moverStoper != null)
            _moverStoper.TurnOnMove();
    }
    
    private void RegisterProps()
    {
        _availableProps = _regulating.GetTo(_counter);
    }

    private IEnumerator MoveBarrelToPoint()
    {
        if (_availableProps.Count == 0) yield break;
        
        while (_barrelPoint.IsFill == false)
        {
            StartCoroutine(_availableProps.Dequeue().TryMoveTo(_barrelPoint));
            yield return null;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MoverStoper moverStoper))
        {
            _moverStoper = moverStoper;
            _moverStoper.TurnOffMove();
            _collider.enabled = false;
            RegisterProps();
            StartCoroutine(MoveBarrelToPoint());
        }
    }

    private void Update()
    {
        Debug.Log(_counter);
    }
}
