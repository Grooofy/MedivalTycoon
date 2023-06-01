using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _barrel;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Regulating _regulating;
    [SerializeField] private int _countBarrels;

    private Queue<GameObject> _barrels = new Queue<GameObject>();

    private void OnEnable()
    {
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
            GameObject newBarrel = Instantiate(_barrel, _startPoint.position, Quaternion.identity);
            _barrels.Enqueue(newBarrel);
        }
        Debug.Log(_barrels.Count);
        SendBarrel();
    }

    private void SendBarrel()
    {
        _regulating.MoveObject(TryGetBarrel());
    }

    private GameObject TryGetBarrel()
    {
        if (_barrels == null)
            return null;

        return _barrels.Dequeue();
    }


}
