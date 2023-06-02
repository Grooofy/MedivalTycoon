using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{


    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bartender bartender))
        {
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }        
}
