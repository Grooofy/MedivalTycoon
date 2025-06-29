using Characters;
using UnityEngine;

public class TableTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Table _table;
    [SerializeField] private Wallet _wallet;

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
        if (other.TryGetComponent(out Bartender wallet))
        {
            _isBuilding = _wallet.TryRemoveCoin(_table.Price);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Bartender wallet) && _isBuilding)
        {
            _wallet.StartRemoveCoins(_table.Price, _step);
            _table.ReducePrice(_step);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Bartender wallet))
        {
            _wallet.StopRemoveCoins();
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