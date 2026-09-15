using System.Linq;
using UnityEngine;

public class LoadingGameSettings : MonoBehaviour
{
    private ISaveSystem _iSaveSystem = new PlayerPrefsSystem();
    private SaveData _saveData;   

    public float GuestWaitSeconds => _saveData.GuestWaitSeconds;
    public int MinBeerAmount => _saveData.MinBeerAmount;
    public int MaxBeerAmount => _saveData.MaxBeerAmount;
    public int LevelNumber => _saveData.NumberLevel;

    public void Load()
    {
        _saveData = _iSaveSystem.Load();
    }

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

    public int GetTableAmount()
    {
        return _saveData.TableAmount;
    }
    
    public int[] GetTableCost()
    {
        return _saveData.TableCost.Split(',').Select(s => int.Parse(s)).ToArray();
    }

    public bool IsTutorialCompleted()
    {
        return _saveData.IsTutorialCompleted;
    }

    public void SaveTutorialStatus(bool completed)
    {
        _saveData.IsTutorialCompleted = completed;
        _iSaveSystem.Save(_saveData);
    }
}
