using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [Header("Barrels")]
    [SerializeField] private Props _barrel;
    [SerializeField] private Transform _spawnBarrelPoint;
    [SerializeField] private Regulating _regulating;
    
    [Header("Beers")]
    [SerializeField] private Props _beer;
    [SerializeField] private Transform _spawnBeerPoint;
    [SerializeField] private BeerCreator _beerCreator;
    
    
    private Queue<Props> _props = new Queue<Props>();
    private readonly int _amount = 30;

    private void Awake()
    {
        CreateObjects(_barrel, _regulating, _spawnBarrelPoint);
        CreateObjects(_beer, _beerCreator, _spawnBeerPoint);
    }

    private void CreateObjects(Props props, IPropsMover mover, Transform spawnPoint)
    {
        for (int i = 0; i < _amount; i++)
        {
            var newProps = Instantiate(props, spawnPoint);
            _props.Enqueue(newProps);
        }
        mover.RegisterProps(_props);
        _props.Clear();
    }
}
