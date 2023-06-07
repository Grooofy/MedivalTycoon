using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;
    [SerializeField] private float _delay;

    private float _delayTime;
    

    private void Start()
    {
        _delayTime = _delay;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
         
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
            _delay -= Time.deltaTime;
            if (_delay < 0)
            {
                bartender.TryTakeObject(_regulating.GetObject());
                _delay = _delayTime;
            }
        }
    }        
}
