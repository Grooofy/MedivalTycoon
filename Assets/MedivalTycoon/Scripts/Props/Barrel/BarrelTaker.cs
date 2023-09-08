using System;
using UnityEngine;

public class BarrelTaker : MonoBehaviour, ITrigger
{
    [SerializeField] Regulating _regulating;
    [SerializeField] SphereCollider _collider;

    private void OnEnable()
    {
        _regulating.Fulling += TunrColider;
    }

    private void OnDisable()
    {
        _regulating.Fulling -= TunrColider;
    }

    private void TunrColider(bool value)
    {
        _collider.enabled = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            bartender.ReceiveObject(_regulating);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Hand bartender))
        {
            bartender.Stop();
        }
    }

    public void OnTriggerStay(Collider other)
    {
       
    }
}
