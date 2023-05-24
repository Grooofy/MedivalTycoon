using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _gameMoney;
    
    private void Awake()
    {
        _wallet.CoinsChanged += ShowTextMoney;
    }

    private void OnDisable()
    {
        _wallet.CoinsChanged -= ShowTextMoney;
    }
    
    private void ShowTextMoney(int number)
    {
        _gameMoney.text = number.ToString();
    }
    
    
    
    
}