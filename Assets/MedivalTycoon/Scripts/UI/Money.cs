using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    [SerializeField] private LoadingGameSettings _loadingGameSettings;
    [SerializeField] private TextMeshProUGUI _gameMoney;

    private void Start()
    {
        ShowTextMoney();
    }

    private void ShowTextMoney()
    {
        _gameMoney.text = _loadingGameSettings.GetMoney().ToString();
    }
}
