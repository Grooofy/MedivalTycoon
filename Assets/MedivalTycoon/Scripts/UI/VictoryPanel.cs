using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public sealed class VictoryPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Image[] _coins;
        [SerializeField] private Button _mainMenu;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private float _panelDuration = 0.5f;
        private Vector2 _panelPosition;
        private Tween _panelTween;
        private bool _panelInitialized;
        private bool _leaving;

        private void OnEnable() => _mainMenu.onClick.AddListener(ReturnToMenu);
        private void OnDisable()
        {
            _mainMenu.onClick.RemoveListener(ReturnToMenu);
            _panelTween?.Kill();
            if (_panelInitialized) _panel.anchoredPosition = _panelPosition;
        }

        public void Show(int result, int earned)
        {
            Localization.LocalizedText.Bind(_rewardText, "victory.reward", earned);
            Localization.LocalizedText.Bind(_resultText, "victory.result", result, LevelRewards.Balance);
            for (int i = 0; i < _coins.Length; i++)
                _coins[i].color = new Color(1f, 1f, 1f, i < result ? 1f : 0.2f);
            _leaving = false;
            _mainMenu.interactable = true;
            gameObject.SetActive(true);
            Time.timeScale = 0f;
            Canvas.ForceUpdateCanvases();
            if (!_panelInitialized)
            {
                _panelPosition = _panel.anchoredPosition;
                _panelInitialized = true;
            }
            _panelTween?.Kill();
            var parent = (RectTransform)_panel.parent;
            _panel.anchoredPosition = _panelPosition + Vector2.up * (parent.rect.height + _panel.rect.height);
            _panelTween = _panel.DOAnchorPos(_panelPosition, _panelDuration)
                .SetEase(Ease.OutBack).SetUpdate(true);
            _mainMenu.Select();
        }

        private void ReturnToMenu()
        {
            if (_leaving) return;
            _leaving = true;
            _mainMenu.interactable = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
