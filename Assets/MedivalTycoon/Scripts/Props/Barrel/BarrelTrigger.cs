using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;


    public void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent(out Hand bartender))
        //{
        //    bartender.TakeObject(_regulating.GetObject());
        //}
    }

    public void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            bartender.TakeObject(_regulating.GetObject());
        }
    }        
}
