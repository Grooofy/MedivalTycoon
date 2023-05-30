using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;


public class Table : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private float _speedBuilding;
    [SerializeField] private ParticleSystem _smoke;

    public TweenCallback LinedUp;
    public UnityAction<int> PriceChanged;
    public int Price => _price;

    private Coroutine _priceChanged;

    public void ReducePrice(int step)
    {
        if (_priceChanged == null)
        {
            _priceChanged = StartCoroutine(ReducesPrice(step));
        }
        else
        {
            StopReducePrice();
            _priceChanged = StartCoroutine(ReducesPrice(step));
        }
    }

    public void StopReducePrice()
    {
        if (_priceChanged != null) 
            StopCoroutine(_priceChanged);
    }
    
    private IEnumerator ReducesPrice(int step)
    {
        while (_price != 0)
        {
            _price -= step;
            PriceChanged?.Invoke(_price);
            yield return null;
        }
        Build();
    }
    
    private void Build()
    {
        _smoke.Play();
        transform.DOScale(Vector3.one, _speedBuilding).OnComplete(LinedUp);
    }
    
}
