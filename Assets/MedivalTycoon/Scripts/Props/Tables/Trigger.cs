using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Table _table;
    
    private bool _isBuilding;
    private bool _go;

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
            wallet.StartRemoveCoins(_table.Price, 1);
            _table.ReducePrice(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Wallet wallet))
        {
            wallet.StopRemoveCoins();
            _table.StopReducePrice();
            _go = true;
        }
    }
    
    private void ActionCollider()
    {
        if (_go)
        {
            _boxCollider.isTrigger = false;
        }
    }
}