using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _barrel;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Regulating _regulating;
    
    private int _countBarrels;

    private Queue<GameObject> _barrels = new Queue<GameObject>();

    private void OnEnable()
    {
        _countBarrels = _regulating.CountBarrel * 2;
        _regulating.BarrelArrived += SendBarrel;
    }

    private void OnDisable()
    {
        _regulating.BarrelArrived -= SendBarrel;
    }

    private void Start()
    {
        CreateObject();
    }

    private void CreateObject()
    {
        for (int i = 0; i < _countBarrels; i++)
        {
            GameObject newBarrel = Instantiate(_barrel, _startPoint.position, _barrel.transform.rotation);
            _barrels.Enqueue(newBarrel);
        }
        SendBarrel();
    }

    private void SendBarrel()
    {
        _regulating.MoveObject(TryGetBarrel());
    }

    private GameObject TryGetBarrel()
    {
        return _barrels.Count == 0 ? null : _barrels.Dequeue();
    }


}
