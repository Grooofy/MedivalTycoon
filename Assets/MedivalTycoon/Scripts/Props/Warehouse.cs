using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warehouse : MonoBehaviour
{
    [SerializeField] private int _capacity;

    private Queue<Barrel> _barrels = new Queue<Barrel>();
    private Barrel[] _barrelsList;   
    

    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            character.PutObjects(GetBarrel());
        }
    }

    private void Awake()
    {
        _barrelsList = GetComponentsInChildren<Barrel>();
        CreateQueue();
    }

    private void Start()
    {
        foreach (var item in _barrels)
        {
            Debug.Log(item.gameObject.name);
        }
        
    }

    private void CreateQueue()
    {
        foreach (var barrel in _barrelsList)
        {
            _barrels.Enqueue(barrel);
        }
    }
    
    private Barrel GetBarrel()
    {
       return  _barrels.Dequeue();
    }

}
