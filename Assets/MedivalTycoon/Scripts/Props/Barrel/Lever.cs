using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject;
    [SerializeField] private Animator _animator;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private SphereCollider _collider;
    private MoverStoper _moverStoper;


    private void OnEnable()
    {
        _regulating.Fulling += TurnObject;
    }

    private void OnDisable()
    {
        _regulating.Fulling -= TurnObject;
    }

   
    private void TurnObject(bool value)
    {
        _animator.SetBool("IsOn", value);
        _collider.enabled = !value;
        _uiObject.SetActive(!value);
        
        if (value)
            _moverStoper.TurnOnMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MoverStoper moverStoped))
        {
            _moverStoper = moverStoped;
            _moverStoper.TurnOffMove();
            _collider.enabled = false;
            _regulating.FillingPoints();
        }
    }
}
