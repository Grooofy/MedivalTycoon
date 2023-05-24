using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;


public class Table : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private int _step;
    [SerializeField] private float _speedBuilding;
    [SerializeField] private ParticleSystem _smoke;

    public TweenCallback LinedUp;
    public UnityAction<int> PriceChanged;
    public int Price => _price;

    public void Build()
    {
        _smoke.Play();
        transform.DOScale(Vector3.one, _speedBuilding).OnComplete(LinedUp);
    }

    private IEnumerator ReducePrice(int newPrice)
    {
        while (_price != newPrice)
        {
            _price -= _step;
            PriceChanged?.Invoke(_price);
            yield return null;
        }
    }
    
}
