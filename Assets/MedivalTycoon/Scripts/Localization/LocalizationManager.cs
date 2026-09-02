using System.Collections.Generic;
using UnityEngine;
namespace Localization
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> _cachedText;
        private static bool _isInitialized;
        public static void Initialize(LocalizationData data)
        {
            if (data == null)
            {
                Debug.LogError("LocalizationManager: LocalizationData is null!");
                return;
            }
            _cachedText = data.GetDictionary();
            _isInitialized = true;
        }
        public static string Get(string id)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("LocalizationManager: Not initialized!");
                return id;
            }
            if (_cachedText.TryGetValue(id, out var text))
            {
                // Заменяем \n на реальные переносы строк, если они есть в тексте
                return text.Replace("\\n", "\n");
            }
            Debug.LogWarning($"LocalizationManager: ID not found: {id}");
            return $"[{id}]";
        }
    }
}