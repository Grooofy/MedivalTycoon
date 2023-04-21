using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingGameSettings : MonoBehaviour
{
    private ISaveSystem _saveSystem = new PlayerPrefsSystem();
    private SaveData _saveData;


    public float GetSeconds()
    {
        return _saveData.Seconds;
    }

    public int GetVisitors()
    {
        return _saveData.NumberVisitors;
    }

    public int GetMoney()
    {
        return _saveData.StartMoney;
    }

    private void Awake()
    {
        _saveData = _saveSystem.Load();
    }

}
