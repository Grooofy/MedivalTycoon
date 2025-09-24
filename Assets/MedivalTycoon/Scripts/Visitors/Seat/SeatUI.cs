using TMPro;
using UnityEngine;

public class SeatUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Seat _seat;


    public void Initialize(Seat seat)
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _seat = seat;
    }


    public void UpdateBeerDisplay(int remaining)
    {
        if (remaining == 0)
            _text.gameObject.SetActive(false);
        else
            _text.gameObject.SetActive(true);

        _text.text = $"Пиво: {remaining}";  
    }

}
