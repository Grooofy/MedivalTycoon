using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.MainMenu
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private LevelPanel _levelPanel;
        [SerializeField] private PanelUI _settingPanel;

        [Header("Scene-authored animated menu (optional for the legacy menu)")]
        [SerializeField] private Button _back;
        [SerializeField] private GameObject _startGameButton;
        [SerializeField] private CanvasGroup _homeButtons;
        [SerializeField] private CanvasGroup _levelActions;
        [SerializeField] private UIAnimation _homeAnimation;
        [SerializeField] private UIAnimation _actionsAnimation;
        [SerializeField] private Toggle _sound;

        private Tween _transition;
        private bool _initialized;
        private bool _transitioning;
        private PanelUI _activePanel;
        private const string SoundPreference = "Menu.SoundEnabled";
        private bool HasAnimation => _homeAnimation != null && _actionsAnimation != null;

        public void Initialize()
        {
            if (_initialized) return;
            Localization.LanguageSelector.Create(GetComponentInParent<Canvas>());
            _start.onClick.AddListener(OpenLevelPanel);
            _settings.onClick.AddListener(OpenSettingsPanel);
            if (_back != null) _back.onClick.AddListener(Back);
            if (_sound != null)
            {
                bool enabled = PlayerPrefs.GetInt(SoundPreference, 1) != 0;
                _sound.SetIsOnWithoutNotify(enabled);
                AudioListener.volume = enabled ? 1f : 0f;
                _sound.onValueChanged.AddListener(SetSound);
            }
            _initialized = true;
            if (HasAnimation) ShowHome();
            else MedivalTycoon.YandexPlatform.MarkReady();
        }

        public void UpdateUI()
        {
             // Здесь можно добавить логику обновления UI, если она понадобится
        }

        private void OnDestroy()
        {
            _transition?.Kill();
            _start.onClick.RemoveListener(OpenLevelPanel);
            _settings.onClick.RemoveListener(OpenSettingsPanel);
            if (_back != null) _back.onClick.RemoveListener(Back);
            if (_sound != null) _sound.onValueChanged.RemoveListener(SetSound);
        }

        private void OnEnable()
        {
            if (_initialized && HasAnimation) ShowHome();
        }

        private void OnDisable()
        {
            _transition?.Kill();
            if (_initialized && HasAnimation)
            {
                _homeAnimation.ResetAnimation();
                _actionsAnimation.ResetAnimation();
            }
        }

        private void ShowHome()
        {
            _transition?.Kill();
            _homeAnimation.ResetAnimation();
            _actionsAnimation.ResetAnimation();
            _levelPanel.HideImmediately();
            _settingPanel.HideImmediately();
            SetGroup(_homeButtons, true, false);
            SetGroup(_levelActions, false, false);
            _transitioning = true;
            _activePanel = null;
            _homeAnimation.Play();
            _transition = DOVirtual.DelayedCall(_homeAnimation.DropDuration + 0.02f, () =>
            {
                SetGroup(_homeButtons, true, true);
                _transitioning = false;
                MedivalTycoon.YandexPlatform.MarkReady();
            });
        }

        private void OpenLevelPanel() => OpenPanel(_levelPanel);

        private void OpenSettingsPanel() => OpenPanel(_settingPanel);

        private void OpenPanel(PanelUI panel)
        {
            if (_transitioning || (HasAnimation && _activePanel != null)) return;
            if (!HasAnimation)
            {
                (panel == _levelPanel ? _settingPanel : _levelPanel).Close();
                panel.Open();
                return;
            }

            _transitioning = true;
            _activePanel = panel;
            (panel == _levelPanel ? _settingPanel : _levelPanel).HideImmediately();
            if (_startGameButton != null) _startGameButton.SetActive(panel == _levelPanel);
            SetGroup(_homeButtons, true, false);
            _homeAnimation.Lift();
            _transition = DOVirtual.DelayedCall(_homeAnimation.LiftDuration, () =>
            {
                SetGroup(_homeButtons, false, false);
                panel.Open();
                SetGroup(_levelActions, true, false);
                _actionsAnimation.Play();
                _transition = DOVirtual.DelayedCall(_actionsAnimation.DropDuration + 0.02f, () =>
                {
                    SetGroup(_levelActions, true, true);
                    _transitioning = false;
                });
            });
        }
        private void Back()
        {
            if (_transitioning || _activePanel == null) return;
            _transitioning = true;
            SetGroup(_levelActions, true, false);
            _activePanel.Close();
            _actionsAnimation.Lift();
            _transition = DOVirtual.DelayedCall(_actionsAnimation.LiftDuration, () =>
            {
                SetGroup(_levelActions, false, false);
                SetGroup(_homeButtons, true, false);
                _homeAnimation.Play();
                _transition = DOVirtual.DelayedCall(_homeAnimation.DropDuration + 0.02f, () =>
                {
                    SetGroup(_homeButtons, true, true);
                    _activePanel = null;
                    _transitioning = false;
                });
            });
        }

        private static void SetGroup(CanvasGroup group, bool visible, bool interactable)
        {
            group.alpha = visible ? 1f : 0f;
            group.interactable = interactable;
            group.blocksRaycasts = interactable;
        }

        private void SetSound(bool enabled)
        {
            AudioListener.volume = enabled ? 1f : 0f;
            PlayerPrefs.SetInt(SoundPreference, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
