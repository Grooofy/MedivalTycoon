using System;
using System.Collections;
using Unity.VisualScripting;
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
    
  
    private void Start()
    {
        LoadCoinsCount();
    }
    
    public void StartAddCoins(int countCoins)
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
    

    public void StartRemoveCoins(int countCoins, int step)
    {
        if (_removedCoins == null)
        {
            _removedCoins = StartCoroutine(RemovedCoins(countCoins, step));
        }
        else
        {
            StopRemoveCoins();
            _removedCoins = StartCoroutine(RemovedCoins(countCoins, step));
        }
    }

    public void StopRemoveCoins()
    {
        if (_removedCoins != null) 
            StopCoroutine(_removedCoins);
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
    
    private IEnumerator RemovedCoins(int number, int step)
    {
        int sum = _coins - number;
        
        while (_coins != sum)
        {
            _coins -= step;
            CoinsChanged?.Invoke(_coins);
            yield return null;
        }
    }
    
    private void LoadCoinsCount()
    {
        _coins =_loadingGameSettings.GetMoney(); 
        CoinsChanged?.Invoke(_coins);
    }
}
