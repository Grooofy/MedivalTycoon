using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    private Wallet _wallet;
    private TextMeshProUGUI _moneyText;
    
    public void Initialize(Wallet wallet)
    {
        _wallet = wallet;
        _moneyText = GetComponentInChildren<TextMeshProUGUI>();
        _wallet.CoinsChanged += ShowTextMoney;
        ShowTextMoney(_wallet.Coins);
    }

    private void OnDestroy()
    {
        _wallet.CoinsChanged -= ShowTextMoney;
    }
    
    private void ShowTextMoney(int number)
    {
        Debug.Log(number + "Стартуем!!");
        _moneyText.text = number.ToString();
    }
    
    
    
    
}