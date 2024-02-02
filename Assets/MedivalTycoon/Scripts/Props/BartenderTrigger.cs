using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartenderTrigger : MonoBehaviour ,ITrigger
{
    [SerializeField] private Regulating _regulating;
    private Hand _hand;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            _hand = hand;
            _hand.ReceiveObject(_regulating);
        }
    }

    public void OnTriggerStay(Collider other)
    {
       
    }

    public void OnTriggerExit(Collider other)
    {
       _hand.Stop();
    }
}
