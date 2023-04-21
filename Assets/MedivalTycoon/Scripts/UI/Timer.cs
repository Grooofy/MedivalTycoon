using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI GameCountTimeText;
    [SerializeField] LoadingGameSettings _loadingGameSettings;  

    private float _gameTime;
    private float timer = 0;
    private int _minut;
    private float _second;

    private void Start()
    {
        _gameTime = _loadingGameSettings.GetSeconds();
    }

    void Update()
    {
        _minut = (int)(_gameTime / 60);
        _second = _gameTime % 60;
        timer += Time.deltaTime;
        ShowTimer();

        if (timer >= 1f)
        {
            timer = 0;
            _gameTime--;
            
            if (_minut <= 0 & _second <= 0)
            {
                
            }
        }
    }

    private void ShowTimer()
    {
        GameCountTimeText.text = _minut + ":" + string.Format("{0:00}", _second);
    }
}