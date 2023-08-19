using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private Barrel _prefab;
    [SerializeField] private Transform _spawmPoint;
    [SerializeField] private int _amount;

    private readonly Queue<Barrel> _barrels = new Queue<Barrel>();
    

    private void Awake()
    {
        for (int i = 0; i < _amount; i++)
        {
            _barrels.Enqueue(CreateBarrel());
        }
    }

    public Barrel GetBarrel()
    {
        if (_barrels != null)
        {
            return _barrels.Dequeue();
        }
        return null;
    }

    private Barrel CreateBarrel()
    {        
        return Instantiate(_prefab, _spawmPoint.position, Quaternion.identity);
    }

}
