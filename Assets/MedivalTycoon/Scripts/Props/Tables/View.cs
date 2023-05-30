using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceBuy;
    [SerializeField] private Table _table;
    [SerializeField] private Slider _slider;
  
        
    private void OnEnable()
    {
        _table.LinedUp += TurnOff;
        _table.PriceChanged += ShowPrice;
    }

    private void OnDisable()
    {
        _table.LinedUp -= TurnOff;
        _table.PriceChanged -= ShowPrice;
    }

    private void Start()
    {
        ShowPrice(_table.Price);
        ConfigureSlider();
    }

    private void ConfigureSlider()
    {
        _slider.minValue = 0;
        _slider.maxValue = _table.Price;
    }

    private void IncreaseSliderValue()
    {
        _slider.value = _slider.maxValue - _table.Price;
    }

    private void ShowPrice(int value)
    {
        _priceBuy.text = value.ToString();
        IncreaseSliderValue();
    }
    
    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
