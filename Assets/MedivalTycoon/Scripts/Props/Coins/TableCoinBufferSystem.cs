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


        public void Initialize(PropsSpawner propsSpawner, TableInteractionMode tableInteractionMode)
        {
            _coinsPool = propsSpawner.GetCoinPool();
            _coinBuffer.Initialize(_coinsPool, tableInteractionMode);  
            _coinGiver.SetActiveGameObject(false);
            _coinGiver.Initialize(_coinBuffer, _layerMask);
            _coinBuffer.AllCoinsCreated += () => _coinGiver.SetActiveGameObject(true);
        }     
        
        public CoinBuffer GetCoinBuffer()
        {
            return _coinBuffer;
        }

        public void CheckHits()
        {
            _coinGiver.CheckHits();
        }


    }
}