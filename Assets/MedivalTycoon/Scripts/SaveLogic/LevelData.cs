using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    private ISaveSystem _saveSystem;
    private SaveData _myData;
    
    private void Awake()
    {
        Init();
    }

    public void Save(Level level)
    {
        FillFields(level);
        _saveSystem.Save(_myData);
    }
    
    private void Init()
    {
        _saveSystem = new PlayerPrefsSystem();
        _myData = new SaveData();
    }
    
    private void FillFields(Level level)
    {
        _myData.NumberLevel = level.NumberLevel;
        _myData.StartMoney = level.StartMoney;
        _myData.NumberVisitors = level.NumberVisitors;
        _myData.Seconds = level.Seconds;
    }
}
