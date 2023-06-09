using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;
    [SerializeField] private float _delay;

    private bool _isMove;
    private Hand _hand;

    private void Start()
    {
       
    }

    private void Update()
    {
        if (_isMove)
        {
            _hand.TryTakeObject(_regulating.GetObject());
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            _hand = bartender;
            _isMove = true;
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
