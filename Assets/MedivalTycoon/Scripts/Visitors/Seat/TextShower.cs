using TMPro;
using UnityEngine;

public class TextShower : MonoBehaviour
{
    private TextMeshProUGUI _text;


    public void Instialize()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }


    private void SetActiveText(bool value, int remainingBeer = 0)
    {
        _text.gameObject.SetActive(value);
        UpdateBeerDisplay(remainingBeer);
    }

    public void UpdateBeerDisplay(int remaining)
    {
        _text.text = $"Пиво: {remaining}";
    }

}
