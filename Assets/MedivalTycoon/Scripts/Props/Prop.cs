using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Prop : MonoBehaviour
{
    [SerializeField] protected int MaxCapacity;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _duration;

    private int _capacity;
    private Vector3 _firstPosition;
    private int _jumpCount = 1;

    private void Awake()
    {
        _firstPosition = transform.position;
    }

    public void ChangeOwner(GameObject owner, Vector3 newPosition)
    {
        ChangePosition(newPosition);
        transform.SetParent(owner.transform);
    }

    public void TryUseProp()
    {
        if (_capacity > 0)
        {
            Use();
        }
        else
        {
            ResetPosition();
        }
    }

    protected void Use()
    {
        _capacity--;
    }

    private void ChangePosition(Vector3 newPosition)
    {
        transform.DOJump(newPosition, _jumpPower, _jumpCount, _duration);
    }

    private void ResetPosition()
    {
        transform.position = _firstPosition;        
        _capacity = MaxCapacity;
    }

}
