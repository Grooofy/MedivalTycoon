using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Character _character;
    
    private Queue<Prop> _myProps = new Queue<Prop>();

   
    public int GetNumberWearableObjects()
    {
        return _character.GetNumberWearableObjects();
    }

    public void TryRaiseObject(Prop prop)
    {
       
    }

    
}
