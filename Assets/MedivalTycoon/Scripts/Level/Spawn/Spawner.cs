using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Props _barrel;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Regulating _regulating;

    private Queue<Props> _props = new Queue<Props>();
    private readonly int _amount = 30;

    private void Awake()
    {
        CreateObjects();
    }

    private void CreateObjects()
    {
        for (int i = 0; i < _amount; i++)
        {
            var newProps = Instantiate(_barrel, _startPoint);
            _props.Enqueue(newProps);
        }
        _regulating.RegisterProps(_props);
    }
}
