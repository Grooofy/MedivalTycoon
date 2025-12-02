using System;
using Characters;
using Tables;
using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    private ConstructionHandler _handler;
    private Table _table;
    private BoxCollider _boxCollider;
    private bool _isBuild;

    public ViewTable Initialize(ConstructionHandler handler)
    {
        _handler = handler;
        _boxCollider = GetComponent<BoxCollider>();
         return GetComponentInChildren<ViewTable>();
    }

    public Table CreateTable(Table prefab)
    {
        _table =  Instantiate(prefab, transform.position, Quaternion.identity, transform);
        _table.LinedUp += EnableCollider;
        return _table;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bartender>(out _))
        {
            _handler.StartBuilding(_table);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Bartender>(out _))
        {
            _handler.StopBuilding();
            
            if (_isBuild)
            {
                _boxCollider.isTrigger = false;
            }
        }
    }

    private void EnableCollider(Seat seat)
    {
        _isBuild = true;
    }

    private void OnDestroy()
    {
        _table.LinedUp -= EnableCollider;
    }
}
