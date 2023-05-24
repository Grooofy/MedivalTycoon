using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Wallet : MonoBehaviour
{
    [SerializeField] private LoadingGameSettings _loadingGameSettings;
    [SerializeField] private int _step;

    public UnityAction<int> CoinsChanged;


    private Coroutine _addedCoins;
    private Coroutine _removedCoins;

    private int _coins;
    
    
    private void Awake()
    {
        LoadCoinsCount();
    }
    
    public void AddCoins(int countCoins)
    {
        if (_addedCoins == null)
        {
            _addedCoins = StartCoroutine(AddedCoins(countCoins));
        }
        else
        {
            StopCoroutine(_addedCoins);
            _addedCoins = StartCoroutine(AddedCoins(countCoins));
        }
    }

    public void RemoveCoins(int countCoins)
    {
        if (_removedCoins == null)
        {
            _removedCoins = StartCoroutine(RemovedCoins(countCoins));
        }
        else
        {
            StopCoroutine(_removedCoins);
            _removedCoins = StartCoroutine(RemovedCoins(countCoins));
        }
    }

    public bool TryRemoveCoin(int priceTable)
    {
        return priceTable <= _coins;
    }

    private IEnumerator AddedCoins(int number)
    {
        while (_coins != number)
        {
            _coins += _step;
            CoinsChanged?.Invoke(_coins);
            yield return null;
        }
    }
    
    private IEnumerator RemovedCoins(int number)
    {
        int sum = _coins - number;
        
        while (_coins != sum)
        {
            _coins -= _step;
            CoinsChanged?.Invoke(_coins);
            yield return null;
        }
    }
    
    private void LoadCoinsCount()
    {
        AddCoins(_loadingGameSettings.GetMoney()); 
    }
}
