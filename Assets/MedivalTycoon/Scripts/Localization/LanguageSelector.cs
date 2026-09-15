using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    public sealed class LanguageSelector : MonoBehaviour
    {
        private static readonly string[] Codes = { "ru", "en", "tr" };
        private static readonly string[] Names = { "Русский", "English", "Türkçe" };
        private readonly Button[] _choices = new Button[3];
        private GameObject _panel;
        private TMP_FontAsset _font;

        public static void Create(Canvas canvas)
        {
            if (canvas == null || canvas.GetComponentInChildren<LanguageSelector>(true) != null) return;
            var host = new GameObject("LanguageSettings", typeof(RectTransform));
            host.layer = canvas.gameObject.layer;
            var rect = (RectTransform)host.transform;
            rect.SetParent(canvas.transform, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one;
            rect.anchoredPosition = new Vector2(-24f, -24f);
            rect.sizeDelta = new Vector2(260f, 60f);
            var selector = host.AddComponent<LanguageSelector>();
            var sample = canvas.GetComponentInChildren<TMP_Text>(true);
            selector._font = sample != null ? sample.font : TMP_Settings.defaultFontAsset;
            selector.Build();
        }

        private void Build()
        {
            var toggle = AddButton(transform, "Settings", "", 0f);
            LocalizedText.Bind(toggle.GetComponentInChildren<TMP_Text>(), "menu.settings");
            var panel = CreateRect("Languages", transform, new Vector2(0f, -68f), new Vector2(260f, 306f));
            _panel = panel.gameObject;
            var background = _panel.AddComponent<Image>();
            background.color = new Color(0.16f, 0.11f, 0.07f, 1f);
            var title = AddLabel(panel, "Title", "", new Vector2(0f, 0f), new Vector2(260f, 48f));
            LocalizedText.Bind(title, "settings.language");
            for (int i = 0; i < Codes.Length; i++)
            {
                string code = Codes[i];
                _choices[i] = AddButton(panel, code, Names[i], -50f - i * 62f);
                _choices[i].onClick.AddListener(() => LocalizationManager.SetUserLanguage(code));
            }
            var close = AddButton(panel, "Close", "", -242f);
            LocalizedText.Bind(close.GetComponentInChildren<TMP_Text>(), "menu.close");
            close.onClick.AddListener(() => _panel.SetActive(false));
            toggle.onClick.AddListener(() =>
            {
                _panel.SetActive(!_panel.activeSelf);
                transform.SetAsLastSibling();
            });
            _panel.SetActive(false);
            Refresh();
        }

        private void OnEnable() => LocalizationManager.LanguageChanged += Refresh;
        private void OnDisable() => LocalizationManager.LanguageChanged -= Refresh;

        private void Refresh()
        {
            for (int i = 0; i < _choices.Length; i++)
                if (_choices[i] != null) _choices[i].interactable = Codes[i] != LocalizationManager.Language;
        }

        private Button AddButton(Transform parent, string name, string text, float y)
        {
            var rect = CreateRect(name, parent, new Vector2(0f, y), new Vector2(260f, 58f));
            var image = rect.gameObject.AddComponent<Image>();
            image.color = new Color(0.42f, 0.29f, 0.13f);
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            AddLabel(rect, "Label", text, Vector2.zero, rect.sizeDelta);
            return button;
        }

        private TMP_Text AddLabel(Transform parent, string name, string text, Vector2 position, Vector2 size)
        {
            var rect = CreateRect(name, parent, position, size);
            var label = rect.gameObject.AddComponent<TextMeshProUGUI>();
            label.font = _font;
            label.fontSize = 28f;
            label.alignment = TextAlignmentOptions.Center;
            label.color = new Color(1f, 0.94f, 0.8f);
            label.raycastTarget = false;
            label.text = text;
            return label;
        }

        private static RectTransform CreateRect(string name, Transform parent, Vector2 position, Vector2 size)
        {
            var obj = new GameObject(name, typeof(RectTransform));
            obj.layer = parent.gameObject.layer;
            var rect = (RectTransform)obj.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = size;
            return rect;
        }
    }
}
