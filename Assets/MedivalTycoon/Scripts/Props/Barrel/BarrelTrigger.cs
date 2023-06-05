using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;
    [SerializeField] private float _delay;

    

    private void Start()
    {
       
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
          _delay -= Time.deltaTime;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            if (_delay < 0)
                bartender.TryTakeObject(_regulating.GetObject());
        }
    }        
}
