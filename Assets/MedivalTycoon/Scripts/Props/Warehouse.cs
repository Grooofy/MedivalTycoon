using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warehouse : MonoBehaviour
{
    private Queue<Barrel> _barrels = new Queue<Barrel>();
    private Barrel[] _barrelsList;  
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Lift character))
        {
            character.TryRaiseObject(TryGetBarrel());
        }
    }

    private void Awake()
    {
        CreateQueue();
    }
    
    private void CreateQueue()
    {
        _barrelsList = GetComponentsInChildren<Barrel>();

        foreach (var barrel in _barrelsList)
        {
            _barrels.Enqueue(barrel);
        }
    }
    
    private Barrel TryGetBarrel()
    {
        if (_barrels.Count == 0)
        {
            return null;
        }
       return  _barrels.Dequeue();
    }

}
