using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    public void ChangePosition(Vector3 newPosition, float _offset)
    {
        var offset = Vector3.up * _offset;
        transform.DOJump(newPosition + offset, _jumpPower, _jumpCount, _duration, false);
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


    private void ResetPosition()
    {
        transform.position = _firstPosition;        
        _capacity = MaxCapacity;
    }

}
