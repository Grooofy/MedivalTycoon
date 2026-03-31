using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private LevelPanel _levelPanel;
        [SerializeField] private SettingPanel _settingPanel;

        public void Initialize()
        {
            _start.onClick.AddListener(OpenLevelPanel);
            _settings.onClick.AddListener(OpenSettingsPanel);
        }

        public void UpdateUI()
        {
            // Здесь можно добавить логику обновления UI, если она понадобится
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveListener(OpenLevelPanel);
            _settings.onClick.RemoveListener(OpenSettingsPanel);
        }

        private void OpenLevelPanel()
        {
            if (_settingPanel.gameObject.activeSelf)
            {
                _settingPanel.Close();
            }
            _levelPanel.Open();
        }

        private void OpenSettingsPanel()
        {
            if (_levelPanel.gameObject.activeSelf)
            {
                _levelPanel.Close();
            }
            _settingPanel.Open();
        }
    }
}
