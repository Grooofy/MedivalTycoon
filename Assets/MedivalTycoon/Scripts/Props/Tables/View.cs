using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceBuy;
    [SerializeField] private Table _table;
    [SerializeField] private Trigger _trigger;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private float _speedFilling;
        
    private void OnEnable()
    {
        _table.LinedUp += TurnOff;
        _trigger.Building += IncreaseSliderValue;
    }

    private void OnDisable()
    {
        _table.LinedUp -= TurnOff;
        _trigger.Building -= IncreaseSliderValue;
    }

    private void Start()
    {
        ShowPrice(_table.Price);
    }

    private void IncreaseSliderValue()
    {
        float step = _speedFilling * Time.deltaTime;
        _slider.value = Mathf.MoveTowards(_slider.value, _slider.maxValue, step);
        int newPrice = (int) (_table.Price - (_table.Price * _slider.value));
        
        ShowPrice(newPrice);
      
        if (_slider.value == _slider.maxValue)
        {
            _table.Build();
        }
    }

    private void ShowPrice(int value)
    {
        _priceBuy.text = value.ToString();
    }
    
    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
