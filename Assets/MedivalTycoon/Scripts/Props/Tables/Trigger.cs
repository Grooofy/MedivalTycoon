using System;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Table _table;

    public UnityAction Building;


    private bool _isBuilding;


    private void OnEnable()
    {
        _table.LinedUp += ActionCollider;
    }

    private void OnDisable()
    {
        _table.LinedUp -= ActionCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wallet wallet))
        {
            _isBuilding = wallet.TryRemoveCoin(_table.Price);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Wallet wallet) && _isBuilding)
        {
            wallet.RemoveCoins(_table.Price);
            Building?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Wallet wallet))
        {
            //_table.TryChangePrice();
        }
    }


    private void ActionCollider()
    {
        _boxCollider.isTrigger = false;
    }
}