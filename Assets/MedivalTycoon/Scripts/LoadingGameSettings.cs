using System.Linq;
using UnityEngine;

public class LoadingGameSettings : MonoBehaviour
{
    private ISaveSystem _iSaveSystem = new PlayerPrefsSystem();
    private SaveData _saveData;

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


}
