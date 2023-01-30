using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warehouse : MonoBehaviour
{
    private Queue<Barrel> _barrels;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            
        }
    }

    private void CreateQueue()
    {
        
    }
    
}
