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
        private bool _leaving;

        private void OnEnable() => _mainMenu.onClick.AddListener(ReturnToMenu);
        private void OnDisable() => _mainMenu.onClick.RemoveListener(ReturnToMenu);

        public void Show(int result, int earned)
        {
            _rewardText.text = $"Получено монет: +{earned}";
            _resultText.text = $"Результат: {result} из 3\nБаланс: {LevelRewards.Balance}" +
                (earned == 0 ? "\nЛучший результат не улучшен" : "");
            for (int i = 0; i < _coins.Length; i++)
                _coins[i].color = new Color(1f, 1f, 1f, i < result ? 1f : 0.2f);
            _leaving = false;
            _mainMenu.interactable = true;
            gameObject.SetActive(true);
            Time.timeScale = 0f;
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
