using UnityEngine;

public class TableTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Table _table;

    private int _step = 1;
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Wallet wallet))
        {
            _isBuilding = wallet.TryRemoveCoin(_table.Price);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Wallet wallet) && _isBuilding)
        {
            wallet.StartRemoveCoins(_table.Price, _step);
            _table.ReducePrice(_step);
        }
    }

    public void OnTriggerExit(Collider other)
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