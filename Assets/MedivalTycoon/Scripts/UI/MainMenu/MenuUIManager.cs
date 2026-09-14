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
        [SerializeField] private CanvasGroup _homeButtons;
        [SerializeField] private CanvasGroup _levelActions;
        [SerializeField] private UIAnimation _homeAnimation;
        [SerializeField] private UIAnimation _actionsAnimation;
        [SerializeField] private Toggle _sound;

        private Tween _transition;
        private bool _initialized;
        private bool _transitioning;
        private bool _showingLevels;
        private const string SoundPreference = "Menu.SoundEnabled";
        private bool HasAnimation => _homeAnimation != null && _actionsAnimation != null;

        public void Initialize()
        {
            if (_initialized) return;
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
            _showingLevels = false;
            _homeAnimation.Play();
            _transition = DOVirtual.DelayedCall(_homeAnimation.DropDuration + 0.02f, () =>
            {
                SetGroup(_homeButtons, true, true);
                _transitioning = false;
            });
        }

        private void OpenLevelPanel()
        {
            if (_transitioning || _showingLevels) return;
            if (HasAnimation)
            {
                _transitioning = true;
                _settingPanel.HideImmediately();
                SetGroup(_homeButtons, true, false);
                _homeAnimation.Lift();
                _transition = DOVirtual.DelayedCall(_homeAnimation.LiftDuration, () =>
                {
                    SetGroup(_homeButtons, false, false);
                    _levelPanel.Open();
                    SetGroup(_levelActions, true, false);
                    _actionsAnimation.Play();
                    _transition = DOVirtual.DelayedCall(_actionsAnimation.DropDuration + 0.02f, () =>
                    {
                        SetGroup(_levelActions, true, true);
                        _showingLevels = true;
                        _transitioning = false;
                    });
                });
                return;
            }
            if (_settingPanel.gameObject.activeSelf)
            {
                _settingPanel.Close();
            }
            _levelPanel.Open();
        }

        private void OpenSettingsPanel()
        {
            if (_transitioning || _showingLevels) return;
            if (_levelPanel.gameObject.activeSelf)
            {
                _levelPanel.Close();
            }
            _settingPanel.Open();
        }

        private void Back()
        {
            if (_transitioning || !_showingLevels) return;
            _transitioning = true;
            SetGroup(_levelActions, true, false);
            _levelPanel.Close();
            _actionsAnimation.Lift();
            _transition = DOVirtual.DelayedCall(_actionsAnimation.LiftDuration, () =>
            {
                SetGroup(_levelActions, false, false);
                SetGroup(_homeButtons, true, false);
                _homeAnimation.Play();
                _transition = DOVirtual.DelayedCall(_homeAnimation.DropDuration + 0.02f, () =>
                {
                    SetGroup(_homeButtons, true, true);
                    _showingLevels = false;
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
