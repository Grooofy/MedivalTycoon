using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Props _barrel;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Regulating _regulating;

    private int _countBarrels = 30;

    private void Awake()
    {
        CreateObject();
    }

    private void CreateObject()
    {
        for (int i = 0; i < _countBarrels; i++)
        {
            var newProps = Instantiate(_barrel, _startPoint.position, _barrel.transform.rotation);
            _regulating.AddProps(newProps);
        }
    }
}
