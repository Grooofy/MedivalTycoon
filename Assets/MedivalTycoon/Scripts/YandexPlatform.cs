using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace MedivalTycoon
{
    // Created before the first scene; survives menu/level transitions.
    public sealed class YandexPlatform : MonoBehaviour
    {
        private static YandexPlatform _instance;
        private bool _initialized, _readyRequested, _readySent;
        private bool _hidden, _platformPaused, _adActive, _suspended;
        private float _savedTimeScale;
        private bool _savedAudioPause;
        private Action<bool> _adFinished;
        private Action _pendingCompletion;
        public static string PlatformLanguage { get; private set; } = "en";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            PlatformLanguage = "en";
            _instance = new GameObject("YandexPlatform").AddComponent<YandexPlatform>();
            DontDestroyOnLoad(_instance.gameObject);
#if UNITY_WEBGL && !UNITY_EDITOR
            MT_Initialize();
#endif
        }

        public static void MarkReady()
        {
            if (_instance == null) return;
            _instance._readyRequested = true;
            _instance.TryReady();
        }

        private void TryReady()
        {
            if (!_initialized || !_readyRequested || _readySent) return;
            _readySent = true;
#if UNITY_WEBGL && !UNITY_EDITOR
            MT_Ready();
#endif
        }

        [Preserve] public void OnSdkInitialized(string language)
        {
            PlatformLanguage = Localization.LocalizationManager.NormalizeLanguage(language);
            Localization.LocalizationManager.SelectLanguage(PlatformLanguage);
            _initialized = true;
            TryReady();
        }

        [Preserve] public void OnSdkError(string message) => Debug.LogWarning("Yandex SDK: " + message);
        [Preserve] public void OnVisibility(string hidden) { _hidden = hidden == "1"; ApplyPause(); }
        [Preserve] public void OnPlatformPause(string paused) { _platformPaused = paused == "1"; ApplyPause(); }

        private void OnApplicationFocus(bool focused)
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _hidden = !focused;
            ApplyPause();
#endif
        }

        private void ApplyPause()
        {
            bool pause = _hidden || _platformPaused || _adActive;
            if (pause && !_suspended)
            {
                _savedTimeScale = Time.timeScale;
                _savedAudioPause = AudioListener.pause;
                _suspended = true;
            }
            if (pause)
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
            }
            else if (_suspended)
            {
                Time.timeScale = _savedTimeScale;
                AudioListener.pause = _savedAudioPause;
                _suspended = false;
            }
        }

        private void LateUpdate()
        {
            ApplyPause();
            if (_suspended || _pendingCompletion == null) return;
            var completion = _pendingCompletion;
            _pendingCompletion = null;
            completion();
        }

        public static bool ShowRewarded(Action<bool> finished)
        {
            var platform = _instance;
            if (platform == null || !platform._initialized || platform._adActive ||
                platform._pendingCompletion != null || platform._suspended) return false;
            platform._adFinished = finished;
            platform._adActive = true;
            platform.ApplyPause();
#if UNITY_WEBGL && !UNITY_EDITOR
            MT_Rewarded();
#endif
            return true;
        }

        [Preserve] public void OnAdFinished(string rewarded)
        {
            if (!_adActive) return;
            var callback = _adFinished;
            _adFinished = null;
            _adActive = false;
            _pendingCompletion = () => callback?.Invoke(rewarded == "1");
            ApplyPause();
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void MT_Initialize();
        [DllImport("__Internal")] private static extern void MT_Ready();
        [DllImport("__Internal")] private static extern void MT_Rewarded();
#endif
    }
}
