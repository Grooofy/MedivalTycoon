using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Vector3 _point;
    
    private Queue<Prop> _myProps = new Queue<Prop>();

   
    public int GetNumberWearableObjects()
    {
        return _character.GetNumberWearableObjects();
    }

    public void TryRaiseObject(Prop prop, float step)
    {
       prop.ChangePosition(_point, step);
    }

    private void Update()
    {
        _point = transform.position;
    }
}
