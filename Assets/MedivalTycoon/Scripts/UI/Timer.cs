using UnityEngine;
using TMPro;
using UnityEngine.Serialization;


public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _gameCountTimeText;
    private float _gameTime;
    private float _tickTimer = 0;
    private int _minutes;
    private float _second;
    private bool _isRunning;

    public void Initialize(LoadingGameSettings loadingGameSettings)
    {
        _gameCountTimeText = GetComponentInChildren<TextMeshProUGUI>();
        _gameTime = loadingGameSettings.GetSeconds();
        _isRunning = true;
    }

    public void UpdateTimer()
    {
        if (_isRunning == false) return;
        
        _minutes =(int)(_gameTime / 60);
        _second = _gameTime % 60;
        _tickTimer += Time.deltaTime;
        ShowTimer();

        if (_tickTimer >= 1f)
        {
            _tickTimer = 0;
            _gameTime--;
            
            if (_minutes <= 0 && _second <= 0)
            {
                
            }
        }
    }
    
    public void Stop()
    {
        _isRunning = false;
    }

    public void Resume()
    {
        _isRunning = true;
    }

    private void ShowTimer()
    {
        _gameCountTimeText.text = string.Format("{0:00}:{1:00}", _minutes, _second); ;
    }
}