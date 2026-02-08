using Lever;
using MedivalTycoon;
using UnityEngine;

namespace Beers
{
    public class BeerManager : MonoBehaviour
    {
        [Header("Barrel CubeSpawnPointSize")] 
        [SerializeField] private Vector3 _spaceSize;
        [SerializeField] private int _spawnCount;
        [SerializeField] private float _spacing;
        [SerializeField] private BeerBuffer _beerBuffer;
        [SerializeField] private BarrelBeerBuffer _barrelBeerBuffer;
        [SerializeField] private LeverInstaller _leverInstaller;
        [SerializeField] private BeerGiver _beerGiver;
        [SerializeField] private BeerTaker _beerTaker;
        [SerializeField] private PropsSpawner _propsSpawner;
        [SerializeField] private int _amountBeerToBarrel;
        [SerializeField] private LayerMask _layerMask;
        
        private IPropsPool _beerPool;

        public void Initialize()
        {
            _beerPool = _propsSpawner.GetBeerPool();
            _beerBuffer.Initialize(_beerPool, _amountBeerToBarrel);
            _beerGiver.Initialize(_beerBuffer, _layerMask);
            _beerTaker.Initialize(_beerBuffer, _layerMask);
            _leverInstaller.InitializeBeerLever(_beerBuffer, _barrelBeerBuffer);
        }

        public void CreatePoints()
        {
            _beerBuffer.CreatePoints(_spawnCount, _spacing, _spaceSize);
        }

        public void CheckHits()
        {
            _beerGiver.CheckHits();
            _beerTaker.CheckHits();
        }
    }
}