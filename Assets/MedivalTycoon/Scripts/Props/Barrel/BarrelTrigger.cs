using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;
    [SerializeField] private float _delay;

    private Hand _hand;

    public void OnTriggerEnter(Collider other)
    {
       
    }

    public void OnTriggerExit(Collider other)
    {
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            _hand = bartender;
            _hand.TryTakeObject(_regulating.GetObject());
        }
    }



}
