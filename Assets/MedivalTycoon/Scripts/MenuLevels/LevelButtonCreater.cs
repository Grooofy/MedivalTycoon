using System.Collections.Generic;
using UnityEngine;

public class LevelButtonCreater : MonoBehaviour
{
    [SerializeField] private LevelBase _levelBase;
    [SerializeField] private Content _content;
    [SerializeField] private LevelUI _levelUI;
    
    private readonly List<LevelUI> _levelIcons = new List<LevelUI>();
    
    private void OnEnable()
    {
        CreateLevelsList();
        SetDataLevel();
    }

    public LevelUI GetLevelButton(int id)
    {
        return _levelIcons[id];
    }

    public int GetIconsCount()
    {
        return _levelIcons.Count;
    }

    public bool GetInfoComplited(int number)
    {
        return _levelBase.IsComplete(number);
    }

    private void CreateLevelsList()
    {
        for (int i = 0; i < _levelBase.LevelsCount; i++)
        {
            var pref = Instantiate(_levelUI.gameObject, _content.transform);
            _levelIcons.Add(CreateLevelUI(pref));
        }
    }

    private void SetDataLevel()
    {
        for (int i = 0; i < _levelBase.LevelsCount; i++)
        {
            _levelIcons[i].SetLevel(_levelBase.GetLevelData(i));
        }
    }
    
    private LevelUI CreateLevelUI(GameObject prefab)
    {
        return prefab.GetComponent<LevelUI>();
    }
}