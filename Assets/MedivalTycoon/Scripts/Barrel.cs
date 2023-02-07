using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Barrel : MonoBehaviour
{
    [SerializeField] private int _beer;
    private float _speedAnimation = 0.5f; 
    private Vector3 _firstPosition;


    public UnityAction PourBeer;

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
        transform.SetParent(owner.transform);
        ChangePosition(newPosition);
    }

    private void ChangePosition(Vector3 newPosition)
    {
       transform.DOMove(newPosition, _speedAnimation);
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
