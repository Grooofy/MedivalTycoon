using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Props _barrel;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Regulating _regulating;

    private readonly int _amount = 30;

    private void Awake()
    {
        CreateObject();
    }

    private void CreateObject()
    {
        for (int i = 0; i < _amount; i++)
        {
            var newProps = Instantiate(_barrel, _startPoint);
            _regulating.RegisterProps(newProps);
        }
    }
}
