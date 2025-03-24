using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelToBeer : MonoBehaviour
{
    [SerializeField] private Regulating _firstSpawn;
    [SerializeField] private Point _barrelPoint;
    [SerializeField] private BeerCreator _beerCreator; 
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;
    
    
    private WaitForSeconds _wait = new WaitForSeconds(0.5f);
    private MoverStoper _moverStoper;
    private Props _currentBarrel;
    

    private void OnEnable()
    {
        _regulating.PointFill += TurnObject;
        _barrelPoint.Filling += EnableMover;
        _beerCreator.Fulling += Reset;
    }

    private void OnDisable()
    {
        _regulating.PointFill -= TurnObject;
        _barrelPoint.Filling -= EnableMover;
        _beerCreator.Fulling -= Reset;
    }


    private void Reset(bool value)
    {
       TurnObject(value);
       _currentBarrel.Reset(_firstSpawn, _barrelPoint);
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
    //Разобраться с выключением Props'ов
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
            //_moverStoper.TurnOffMove();
            _collider.enabled = false;
            StartCoroutine(FillingPoints());
        }
    }

    public IEnumerator FillingPoints()
    {
        var currentQueueBarrels = _regulating.GetTo(1);
        if (currentQueueBarrels.Count == 0) yield break;
        
        _currentBarrel = currentQueueBarrels.Peek();
        
        while (_barrelPoint.IsFill == false)
        {
            if (_currentBarrel == null) yield break;
            
            StartCoroutine(_currentBarrel.TryMoveTo(_barrelPoint));
            _currentBarrel.ScaleUp();
            StartCoroutine(_beerCreator.FillingPoints());
            yield return _wait;
        }
    }
}
