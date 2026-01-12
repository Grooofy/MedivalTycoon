using Beers;
using Lever;
using MedivalTycoon;
using System.Collections;
using UnityEngine;

namespace Money
{
    public class CoinManager : MonoBehaviour
    {
        [Header("Coin ColumeSpawnPointSize")]        
        [SerializeField] private float _spacing;
        [SerializeField] private CoinBuffer _coinBuffer;
        [SerializeField] private CoinGiver _coinGiver;
        
        [SerializeField] private LayerMask _layerMask;

        private IPropsPool _coinsPool;


        public void Initialize(PropsSpawner propsSpawner)
        {
            _coinsPool = propsSpawner.GetCoinPool();
            _coinBuffer.Initialize(_coinsPool);           
            _coinGiver.Initialize(_coinBuffer, _layerMask);
        }     
        
        public IPropsMover GetIProps()
        {
            return _coinBuffer;
        }

        public void CheckHits()
        {
            _coinGiver.CheckHits();
        }


    }
}