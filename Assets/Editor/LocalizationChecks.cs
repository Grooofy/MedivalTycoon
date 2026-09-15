using System;
using Localization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public static class LocalizationChecks
{
    private const string BatchKey = "LocalizationChecks.Batch";

    static LocalizationChecks()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool(BatchKey, false)) return;
            SessionState.SetBool(BatchKey, false);
            EditorApplication.delayCall += () =>
            {
                try { Run(); EditorApplication.Exit(0); }
                catch (Exception error) { Debug.LogException(error); EditorApplication.Exit(1); }
            };
        };
    }

    // CLI: -batchmode -nographics -executeMethod LocalizationChecks.RunBatch (without -quit).
    public static void RunBatch()
    {
        SessionState.SetBool(BatchKey, true);
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("Tools/Localization/Run behavior checks", true)]
    private static bool CanRun() => EditorApplication.isPlaying;

    [MenuItem("Tools/Localization/Run behavior checks")]
    public static void Run()
    {
        if (!Application.isPlaying) throw new InvalidOperationException("Run behavior checks in Play mode.");
        LocalizationValidation.Validate();
        bool hadPreference = PlayerPrefs.HasKey(LocalizationManager.PreferenceKey);
        string preference = PlayerPrefs.GetString(LocalizationManager.PreferenceKey);
        string previousLanguage = LocalizationManager.Language;
        var obj = new GameObject("Localization check", typeof(RectTransform), typeof(Text));
        var empty = ScriptableObject.CreateInstance<LocalizationData>();
        int changes = 0;
        Action changed = () => changes++;
        try
        {
            PlayerPrefs.DeleteKey(LocalizationManager.PreferenceKey);
            LocalizationManager.SelectLanguage("RU-ru");
            Check(LocalizationManager.Language == "ru", "Normalize platform language");
            var label = obj.GetComponent<Text>();
            LocalizedText.Bind(label, "victory.reward", 12);
            Check(label.text == "Получено монет: +12", "Russian formatted label");
            LocalizationManager.LanguageChanged += changed;
            LocalizationManager.SetUserLanguage("tr-TR");
            Check(label.text == "Kazanılan altın: +12", "Refresh active label and preserve arguments");
            Check(PlayerPrefs.GetString(LocalizationManager.PreferenceKey) == "tr", "Save normalized choice");
            LocalizationManager.SelectLanguage("en");
            Check(LocalizationManager.Language == "tr", "Late platform response preserves user choice");
            Check(changes == 1, "No duplicate notifications for unchanged language");
            obj.SetActive(false);
            LocalizationManager.SetUserLanguage("en");
            obj.SetActive(true);
            Check(label.text == "Coins earned: +12", "Refresh re-enabled label");
            obj.SetActive(false);
            LocalizedText.Bind(label, "seat.beer", 3);
            LocalizationManager.SetUserLanguage("ru");
            obj.SetActive(true);
            Check(label.text == "Пиво: 3", "Bind and switch while hidden");
            UnityEngine.Object.DestroyImmediate(obj);
            LocalizationManager.Initialize(empty);
            Check(LocalizationManager.Get("menu.play") == "Play", "Per-key English fallback");
            PlayerPrefs.DeleteKey(LocalizationManager.PreferenceKey);
            LocalizationManager.SelectLanguage("de-DE");
            Check(LocalizationManager.Language == "en", "Unsupported language fallback");
            Check(LocalizationManager.NormalizeLanguage("TR_tr") == "tr", "Underscore locale");
            Check(LocalizationManager.NormalizeLanguage(null) == "en", "Missing locale");
            Debug.Log("Localization behavior checks passed.");
        }
        finally
        {
            LocalizationManager.LanguageChanged -= changed;
            if (obj != null) UnityEngine.Object.DestroyImmediate(obj);
            UnityEngine.Object.DestroyImmediate(empty);
            if (hadPreference) PlayerPrefs.SetString(LocalizationManager.PreferenceKey, preference);
            else PlayerPrefs.DeleteKey(LocalizationManager.PreferenceKey);
            PlayerPrefs.Save();
            // Force reload even when the synthetic test dictionary has the same language.
            LocalizationManager.Initialize(Resources.Load<LocalizationData>("Localization/" + previousLanguage));
            LocalizationManager.SelectLanguage(previousLanguage);
        }
    }

    private static void Check(bool passed, string description)
    {
        if (!passed) throw new InvalidOperationException("Localization check failed: " + description);
    }
}
