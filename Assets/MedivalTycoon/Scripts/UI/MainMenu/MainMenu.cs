using System;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private Button _settings;
    [SerializeField] private LevelPanel _levelPanel;
    [SerializeField] private SettingPanel _settingPanel;
 
    private void OnEnable()
    {
        _start.onClick.AddListener(OpenLevelPanel); 
        _settings.onClick.AddListener(OpenSettingsPanel); 
    }

    private void OnDisable()
    {
        _start.onClick.RemoveListener(OpenLevelPanel); 
        _settings.onClick.RemoveListener(OpenSettingsPanel); 
    }

    private void OpenLevelPanel()
    {
        if (_settingPanel.enabled)
        {
            _settingPanel.Close();
        }
        _levelPanel.Open();
    }

    private void OpenSettingsPanel()
    {
        if (_levelPanel.enabled)
        {
            _levelPanel.Close();
        }
        _settingPanel.Open();
    }
}