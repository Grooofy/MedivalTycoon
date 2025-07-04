using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Tables
{
    public class ViewTable : MonoBehaviour
    {
        private TextMeshProUGUI _priceText;
        private Slider _slider;
        private TableBuilderAnimation _tableBuilder;
        private Table _table;

        internal void Initialize(Table table, TableBuilderAnimation tableBuilder)
        {
            _slider = GetComponentInChildren<Slider>();
            _priceText = GetComponentInChildren<TextMeshProUGUI>();
            _table = table;
            _tableBuilder = tableBuilder;
            _table.PriceChanged += ShowPrice;
            _table.LinedUp += Hide;
            _table.LinedUp += OnBuilt;
            _slider.minValue = 0;
            _slider.maxValue = Convert.ToSingle(_table.Price);
            ShowPrice(_table.Price);
        }

        private void ShowPrice(int price)
        {
            _priceText.text = price.ToString();
            _slider.value = _slider.maxValue - price;
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnBuilt()
        {
            _tableBuilder.Play(_table);
        }

        private void OnDestroy()
        {
            _table.PriceChanged -= ShowPrice;
            _table.LinedUp -= Hide;
            _table.LinedUp -= OnBuilt;
        }
    }
}