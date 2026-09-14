using Beers;
using Money;
using UnityEngine;


public class TableInteractionMode : MonoBehaviour
{
    private BeerTaker _beerTaker;
    private bool _isBeerMode;

    public void Initialize(BeerTaker beerTaker)
    {
        _beerTaker = beerTaker;

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
    }
}




