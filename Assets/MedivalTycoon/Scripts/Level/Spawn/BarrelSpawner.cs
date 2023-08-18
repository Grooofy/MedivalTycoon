using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private Barrel _prefab;
    [SerializeField] private Transform _spawmPoint;
    [SerializeField] private int _amount;

    private Queue<Barrel> barrels = new Queue<Barrel>();
    

    private void Awake()
    {
        for (int i = 0; i < _amount; i++)
        {
            barrels.Enqueue(CreateBarrel());
        }
    }

    private Barrel CreateBarrel()
    {
        return Instantiate(_prefab, _spawmPoint.position, Quaternion.identity);
    }

}
