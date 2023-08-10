using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;
    [SerializeField] private float _delay;

    private bool _isMove;
    private Hand _hand;

   private void Update()
    {
        if (_isMove)
        {
            _hand.TryTakeObject(_regulating.GetObject());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        bool isTach = true;
        
        if (isTach)
        {
            if (other.TryGetComponent(out Hand bartender))
            {
                _hand = bartender;
                _isMove = true;
                isTach = false;
            }
        }
       
    }

    public void OnTriggerExit(Collider other)
    {
        _isMove = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            _hand = bartender;
            _isMove = true;
        }
    }    
    

    
}
