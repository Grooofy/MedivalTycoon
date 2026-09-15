using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Localization
{
    public static class LocalizationManager
    {
        public const string PreferenceKey = "Localization.Language";
        public static event Action LanguageChanged;
        private static Dictionary<string, string> _cachedText;
        private static Dictionary<string, string> _fallback;
        public static string Language { get; private set; } = "en";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _cachedText = null;
            _fallback = null;
            LanguageChanged = null;
            Language = "en";
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() => EnsureInitialized();

        public static string NormalizeLanguage(string code)
        {
            var language = (code ?? "").Trim().Replace('_', '-').Split('-')[0].ToLowerInvariant();
            return language == "ru" || language == "tr" ? language : "en";
        }

        public static void EnsureInitialized()
        {
            if (_cachedText == null) SelectLanguage("en");
        }

        // Platform detection must never override an explicit player preference.
        public static void SelectLanguage(string platformLanguage)
        {
            ApplyLanguage(PlayerPrefs.HasKey(PreferenceKey)
                ? PlayerPrefs.GetString(PreferenceKey) : platformLanguage);
        }

        public static void SetUserLanguage(string language)
        {
            language = NormalizeLanguage(language);
            PlayerPrefs.SetString(PreferenceKey, language);
            PlayerPrefs.Save();
            ApplyLanguage(language);
        }

        private static void ApplyLanguage(string language)
        {
            language = NormalizeLanguage(language);
            if (_cachedText != null && Language == language) return;
            var fallback = Resources.Load<LocalizationData>("Localization/en");
            _fallback = fallback != null ? fallback.GetDictionary() : new Dictionary<string, string>();
            var data = Resources.Load<LocalizationData>("Localization/" + language);
            Language = data != null ? language : "en";
            _cachedText = data != null ? data.GetDictionary() : _fallback;
            if (data == null) Debug.LogError("Missing localization dictionary: " + language);
            LanguageChanged?.Invoke();
        }

        public static void Initialize(LocalizationData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            _cachedText = data.GetDictionary();
            var fallback = Resources.Load<LocalizationData>("Localization/en");
            _fallback = fallback != null ? fallback.GetDictionary() : new Dictionary<string, string>();
            LanguageChanged?.Invoke();
        }

        public static string Get(string id)
        {
            EnsureInitialized();
            if (!string.IsNullOrEmpty(id))
            {
                if (_cachedText.TryGetValue(id, out var text) && !string.IsNullOrEmpty(text))
                    return text.Replace("\\n", "\n");
                if (_fallback.TryGetValue(id, out text) && !string.IsNullOrEmpty(text))
                    return text.Replace("\\n", "\n");
            }
            Debug.LogWarning("Localization key not found: " + id);
            return "[" + id + "]";
        }

        public static string Format(string id, params object[] arguments) =>
            string.Format(CultureInfo.InvariantCulture, Get(id), arguments);
    }
}
