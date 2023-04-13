using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonCreater : MonoBehaviour
{
    [SerializeField] private int _countLevels;
    [SerializeField] private Content _content;
    [SerializeField] private LevelUI _levelUI;
    
    private readonly List<LevelUI> _levelIcons = new List<LevelUI>();

    private void OnEnable()
    {
        CreateLevelsList();
    }

    public LevelUI GetLevelButton(int id)
    {
        return _levelIcons[id];
    }

    private void CreateLevelsList()
    {
        for (int i = 0; i < _countLevels; i++)
        {
            _levelIcons.Add(CreateIcon());
        }
    }
    
    private LevelUI CreateIcon()
    {
        var pref = Instantiate(_levelUI.gameObject, _content.transform);
        LevelUI ui = pref.GetComponent<LevelUI>();
        return ui;
    }
}