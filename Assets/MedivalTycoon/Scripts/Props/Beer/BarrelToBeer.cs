using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BarrelToBeer : MonoBehaviour, IPropsMover
{
    [SerializeField] private Point _barrelPoint;
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;
    
    
    private WaitForSeconds _wait = new WaitForSeconds(0.5f);
    private MoverStoper _moverStoper;


    private void OnEnable()
    {
        _regulating.PointFill += TurnObject;
        _barrelPoint.Filling += EnableMover;
    }

    private void OnDisable()
    {
        _regulating.PointFill -= TurnObject;
        _barrelPoint.Filling -= EnableMover;
    }

    private void EnableMover(bool value)
    {
        _animator.SetBool("IsOn", value);
        _collider.enabled = !value;
        _uiObject.SetActive(!value);
        
        if (_moverStoper != null)
        {
            _moverStoper.TurnOnMove();
            _moverStoper = null;
        }
    }
    
    private void TurnObject(bool value)
    {
        _animator.SetBool("IsOn", value);
        _collider.enabled = value;
        _uiObject.SetActive(value);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MoverStoper moverStoper))
        {
            _moverStoper = moverStoper;
            _moverStoper.TurnOffMove();
            _collider.enabled = false;
            StartCoroutine(FillingPoints());
        }
    }


    public IEnumerator FillingPoints()
    {
        var currentQueueBarrels = _regulating.GetTo(1);
        var currentBarrel = currentQueueBarrels.Peek();
        
        while (_barrelPoint.IsFill == false)
        {
            if (currentBarrel == null) yield break;
            
            StartCoroutine(currentBarrel.TryMoveTo(_barrelPoint));
            currentBarrel.ScaleUp();
            yield return _wait;
        }
    }
    
    
    public void RegisterProps(Queue<Props> props) { }

    public Queue<Props> GetTo(int amount) { return null; }
}
