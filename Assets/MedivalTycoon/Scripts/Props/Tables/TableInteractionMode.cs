using Beers;
using Money;
using UnityEngine;


public class TableInteractionMode : MonoBehaviour
{
    private BeerTaker _beerTaker;
    private CoinGiver _coinGiver;
    private bool _isBeerMode;

    public void Initialize(BeerTaker beerTaker)
    {
        _beerTaker = beerTaker;
        _coinGiver = GetComponentInChildren<CoinGiver>();

        SetBeerMode(false);
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




