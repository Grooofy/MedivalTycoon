using System.Collections.Generic;
using Propses;
using UnityEngine;
using UnityEngine.Serialization;

public class PropsSpawner : MonoBehaviour
{
    [Header("Barrels")]
    [SerializeField] private Barrel _barrel;
    [SerializeField] private Transform _spawnBarrelPoint;
    [SerializeField] private BarrelBuffer barrelBuffer;
    
    
    [Header("Beers")]
    [SerializeField] private Barrel _beer;
    [SerializeField] private Transform _spawnBeerPoint;
    [SerializeField] private BeerCreator _beerCreator;
    
    
    private Queue<IProps> _props = new Queue<IProps>();
    private readonly int _amount = 30;

    private void Awake()
    {
        barrelBuffer.Initialize("Barrel", new Vector3(2,1,2));
        barrelBuffer.CreatePoints(9,0.5f);
        CreateObjects(_barrel, barrelBuffer, _spawnBarrelPoint);
        CreateObjects(_beer, _beerCreator, _spawnBeerPoint);
    }

    private void CreateObjects(Barrel barrel, IPropsMover mover, Transform spawnPoint)
    {
        for (int i = 0; i < _amount; i++)
        {
            var newProps = Instantiate(barrel, spawnPoint);
            var animator = newProps.GetComponent<Animator>();
            newProps.Initilization(_spawnBarrelPoint, 3, animator);
            _props.Enqueue(newProps);
        }
        mover.RegisterProps(_props);
        _props.Clear();
    }
}
