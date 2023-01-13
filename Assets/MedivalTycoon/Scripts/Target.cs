using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [SerializeField] private List<SwitchInputController> _switchers;
    public UnityAction<int> TargetReady;

    private void OnEnable()
    {
        foreach (var switcher in _switchers)
        {
            switcher.Activate += TargetReady;
        }
    }

    private void OnDisable()
    {
        foreach (var switcher in _switchers)
        {
            switcher.Activate -= TargetReady;
        }
    }


    public Transform Get(int id)
    {
        return _switchers[id].transform;
    }
    
}