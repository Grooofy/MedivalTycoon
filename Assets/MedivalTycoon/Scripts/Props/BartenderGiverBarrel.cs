using System;
using UnityEngine;

public class BartenderGiverBarrel : MonoBehaviour, ITrigger
{
    [SerializeField] private Regulating _regulating;
    private Hand _hand;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand hand))
        {
            _hand = hand;
            _hand.RemoveObject(_regulating);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}
