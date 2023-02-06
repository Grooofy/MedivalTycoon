using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warehouse : MonoBehaviour
{
    [SerializeField] private int _capacity;

    private Queue<Barrel> _barrels = new Queue<Barrel>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            character.PutObject(GetBarrel());
        }
    }

    private void Awake()
    {
        CreateQueue();
    }

    private void Start()
    {
        Debug.Log(_barrels.Count);
    }

    private void CreateQueue()
    {
        for (int i = 0; i < _capacity; i++)
        {
            _barrels.Enqueue(GetComponentInChildren<Barrel>());
        }
    }
    
    private Barrel GetBarrel()
    {
       return  _barrels.Dequeue();
    }

}
