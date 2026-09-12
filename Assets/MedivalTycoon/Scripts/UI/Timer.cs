using UnityEngine;
using TMPro;
using System;


public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _gameCountTimeText;
    private float _gameTime;
    private int _minutes;
    private float _second;
    private bool _isRunning;
    private Action _onTimeOut;

    public void Initialize(LoadingGameSettings loadingGameSettings, Action onTimeOut)
    {
        _gameCountTimeText = GetComponentInChildren<TextMeshProUGUI>();
        _gameTime = Mathf.Max(0f, loadingGameSettings.GetSeconds());
        _onTimeOut = onTimeOut;
        _isRunning = true;
        ShowTimer();
    }

    public void UpdateTimer()
    {
        if (_isRunning == false) return;
        
        SubtractSeconds(Time.deltaTime);
    }

    public void SubtractSeconds(float seconds)
    {
        if (!_isRunning || seconds < 0f) return;

        _gameTime = Mathf.Max(0f, _gameTime - seconds);
        ShowTimer();

        if (_gameTime <= 0f)
        {
            _isRunning = false;
            _onTimeOut?.Invoke();
        }
    }
    
    public void Stop()
    {
        _isRunning = false;
    }

    public void Resume()
    {
        _isRunning = _gameTime > 0f;
    }

    private void ShowTimer()
    {
        int seconds = Mathf.CeilToInt(_gameTime);
        _minutes = seconds / 60;
        _second = seconds % 60;
        _gameCountTimeText.text = string.Format("{0:00}:{1:00}", _minutes, _second);
    }
}
