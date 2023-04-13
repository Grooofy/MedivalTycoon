using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private LevelBase _levelBase;
    private ISaveSystem _saveSystem;
    private SaveData _myData;
    private Level _level;

    private void OnEnable()
    {
        _button.onClick.AddListener(Save);
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _levelBase = GetComponentInParent<LevelBase>();
        _saveSystem = new PlayerPrefsSystem();
        _myData = new SaveData();
        _level = _levelBase.GetLevelData(0);
    }

    private void Save()
    {
        FillFields();
        _saveSystem.Save(_myData);
    }

    private void FillFields()
    {
        _myData.NumberLevel = _level.NumberLevel;
        _myData.StartMoney = _level.StartMoney;
        _myData.NumberVisitors = _level.NumberVisitors;
        _myData.Seconds = _level.Seconds;
    }
}
