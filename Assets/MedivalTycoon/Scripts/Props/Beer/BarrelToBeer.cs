using UnityEngine;

public class BarrelToBeer : MonoBehaviour
{
    [SerializeField] private Regulating _regulating;
    [SerializeField] private BeerCreator _beerCreator;

private int counter = 0;
    private void OnEnable()
    {
        _regulating.BarrelAmount += ShowAmount;
    }

    private void OnDisable()
    {
        _regulating.BarrelAmount -= ShowAmount;
    }

    private void ShowAmount(int _obj)
    {
        counter += _obj;
        Debug.Log(counter + " added to BarrelAmount");
    }
}
