using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Barrel : MonoBehaviour
{
    [SerializeField] private int _beer;
    private Vector3 _firstPosition;

    public UnityEvent PourBeer;

    private void Awake()
    {
        _firstPosition = transform.position;
    }

    public void TryPourBeer()
    {
        if (_beer > 0)
        {
            Pour();
        }
        else
        {
            ResetPosition();
        }
    }

    public void ChangeOwner(GameObject owner, Vector3 newPosition)
    {
        ChangePosition(newPosition);
        gameObject.transform.SetParent(owner.transform);
    }

    private void ChangePosition(Vector3 newPosition)
    {
       transform.DOMove(newPosition, 1);
    }
    
    private void Pour()
    {
        PourBeer?.Invoke();
        _beer--;
    }

    private void ResetPosition()
    {
        transform.position = _firstPosition;
    }
    
    
}
