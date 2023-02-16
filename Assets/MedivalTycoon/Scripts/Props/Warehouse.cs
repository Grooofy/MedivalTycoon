using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warehouse : MonoBehaviour
{
    private Queue<Barrel> _barrels = new Queue<Barrel>();
    private Barrel[] _barrelsList;
    private WaitForSeconds _delay = new WaitForSeconds(0.25f);
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Lift lift))
        {
            StartCoroutine(RaiseObjects(lift, _delay));
        }
    }

    private void Awake()
    {
        CreateQueue();
    }

    private IEnumerator RaiseObjects(Lift lift, WaitForSeconds delay)
    {
        while (_barrels.Count != 0)
        {
            lift.TryRaiseObject(TryGetBarrel());

            yield return delay;
        }
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
