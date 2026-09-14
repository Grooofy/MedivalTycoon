using System.Collections.Generic;
using UnityEngine;

public class LevelButtonCreater : MonoBehaviour
{
    [SerializeField] private LevelBase _levelBase;
    [SerializeField] private Content _content;
    [SerializeField] private LevelUI _levelUI;
    [SerializeField] private LevelUI _levelUITutorial;
    [SerializeField] private List<LevelUI> _sceneLevelIcons = new List<LevelUI>();
    
    private readonly List<LevelUI> _levelIcons = new List<LevelUI>();
    private bool _initialized;
    
    private void Start()
    {
        // Initialize(); // Будет вызываться из Menu.cs
    }

    public void Initialize()
    {
        if (_initialized) return;
        if (_sceneLevelIcons.Count > 0)
            _levelIcons.AddRange(_sceneLevelIcons);
        else
            CreateLevelsList();
        SetDataLevel();
        _initialized = true;
        if (_sceneLevelIcons.Count > 0)
        {
            for (int i = 0; i < _levelIcons.Count; i++)
            {
                if (!CanStart(i)) continue;
                _levelIcons[i].SelectLevel();
                break;
            }
        }
    }

    public LevelUI GetLevelButton(int id)
    {
        return _levelIcons[id];
    }

    public int GetIconsCount()
    {
        return _levelIcons.Count;
    }

    public bool GetInfoCompleted(int number)
    {
        return _levelBase.IsComplete(number);
    }

    public bool CanStart(int id)
    {
        return _levelBase.CanStart(id);
    }

    private void CreateLevelsList()
    {
        _levelIcons.Add(_levelUITutorial);

        for (int i = 1; i < _levelBase.LevelsCount; i++)
        {
            var pref = Instantiate(_levelUI.gameObject, _content.transform);
            _levelIcons.Add(CreateLevelUI(pref));
        }
    }

    private void SetDataLevel()
    {
        for (int i = 0; i < _levelBase.LevelsCount; i++)
        {
            _levelIcons[i].SetAvailability(CanStart(i), GetInfoCompleted(i));
            if (i == 0)
                _levelIcons[i].SetLevel(_levelBase.GetTutorLevelData());
            else 
                _levelIcons[i].SetLevel(_levelBase.GetLevelData(i));
        }
    }
    
    private LevelUI CreateLevelUI(GameObject prefab)
    {
        return prefab.GetComponent<LevelUI>();
    }
}
