using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [DisallowMultipleComponent]
    public sealed class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        private TMP_Text _tmp;
        private Text _legacy;
        private object[] _arguments;
        public string Key => _key;

        private void Awake()
        {
            _tmp = GetComponent<TMP_Text>();
            _legacy = GetComponent<Text>();
        }

        private void OnEnable()
        {
            LocalizationManager.LanguageChanged += Refresh;
            Refresh();
        }

        private void OnDisable() => LocalizationManager.LanguageChanged -= Refresh;

        public void SetKey(string key, params object[] arguments)
        {
            _key = key;
            _arguments = arguments;
            Refresh();
        }

        public static void Bind(Component text, string key, params object[] arguments)
        {
            var localized = text.GetComponent<LocalizedText>();
            if (localized == null) localized = text.gameObject.AddComponent<LocalizedText>();
            localized.SetKey(key, arguments);
        }

        private void Refresh()
        {
            if (string.IsNullOrEmpty(_key)) return;
            // Bind can be called before Awake on an inactive result/tutorial panel.
            if (_tmp == null) _tmp = GetComponent<TMP_Text>();
            if (_legacy == null) _legacy = GetComponent<Text>();
            var value = _arguments == null || _arguments.Length == 0
                ? LocalizationManager.Get(_key) : LocalizationManager.Format(_key, _arguments);
            if (_tmp != null) _tmp.text = value;
            if (_legacy != null) _legacy.text = value;
        }
    }
}
