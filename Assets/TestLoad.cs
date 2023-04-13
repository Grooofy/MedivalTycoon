using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    private ISaveSystem _saveSystem = new PlayerPrefsSystem();
    private SaveData _saveData;
    
    private void Start()
    {
        _saveData = _saveSystem.Load();
        
        Debug.Log("Уровень " +_saveData.NumberLevel);
        Debug.Log("Деньги " +_saveData.StartMoney);
        Debug.Log("Поситители " +_saveData.NumberVisitors);
        Debug.Log("Время " +_saveData.Seconds);
    }
}
