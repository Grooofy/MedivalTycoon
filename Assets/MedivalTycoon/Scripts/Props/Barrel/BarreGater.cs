using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarreGater : MonoBehaviour, ITrigger
{
    [SerializeField] private Regulating _regulating;
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            _regulating.FillinPoint();
        }
    }

    public void OnTriggerExit(Collider other)
    {
       
    }

    public void OnTriggerStay(Collider other)
    {
        
    }
}
