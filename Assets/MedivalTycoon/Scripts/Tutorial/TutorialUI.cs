using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Tutorial
{
    public class TutorialUI : PanelUI
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Image _icon;

        private Action _onNextClick;

        public void ShowMessage(string text, Action onNext, Sprite icon = null)
        {            
            _messageText.text = text;
            _onNextClick = onNext;
            _icon.sprite = icon;
            _icon.gameObject.SetActive(icon != null);
            
            _nextButton.onClick.RemoveAllListeners();
            _nextButton.onClick.AddListener(() => _onNextClick?.Invoke());
            
            // Перемещаем окно в конец иерархии, чтобы оно было поверх Spotlight
            transform.SetAsLastSibling();

            Open();
        }
        public override void Close()
        {
            base.Close();
            _nextButton.onClick.RemoveAllListeners();
        }
    }
}
