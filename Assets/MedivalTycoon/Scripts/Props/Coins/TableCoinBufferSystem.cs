using Beers;
using Lever;
using MedivalTycoon;
using System.Collections;
using UnityEngine;

namespace Money
{
    public class TableCoinBufferSystem : MonoBehaviour
    {   
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