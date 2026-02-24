using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    public UnityAction<int> CoinsChanged;
    public int Coins => _coins;

    private LoadingGameSettings _loadingGameSettings;
    private int _step = 1;
    private int _coins;
    
    private Coroutine _addedCoins;
    private Coroutine _removedCoins;
  
    public void Initialize(LoadingGameSettings loadingGameSettings)
    {
        _loadingGameSettings = loadingGameSettings;
        LoadCoinsCount();
    }
    
    public void StartAddCoins(int countCoins)
    {
        Debug.Log(countCoins + " Прибавить");
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

    private void StopRemoveCoins()
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
        int sum = _coins + number;

        while (_coins != sum)
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
