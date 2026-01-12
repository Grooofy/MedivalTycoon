using Beers;
using Money;
using UnityEngine;

namespace Tables
{
    public class TableInteractionMode : MonoBehaviour
    {
        private BeerTaker _beerTaker;
        private CoinGiver _coinGiver;
        private bool _isBeerMode;

        public void Initialize()
        {
            _beerTaker = GetComponentInChildren<BeerTaker>();
            _coinGiver = GetComponentInChildren<CoinGiver>();

            SetBeerMode(true);
        }

        public void Switch()
        {
            SetBeerMode(!_isBeerMode);
        }

        private void SetBeerMode(bool beerMode)
        {
            _isBeerMode = beerMode;

            _beerTaker.SetActiveGameObject(_isBeerMode);
            _coinGiver.SetActiveGameObject(!_isBeerMode);
        }
    }
}



