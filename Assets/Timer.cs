using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time = 10000;
    public bool start = false;
    public TextMeshProUGUI value;

    public void StartTimer()
    {
        start = true;
    }
    public void PauseTimer()
    {
        start = false;
    }
    public void StopTimer()
    {
        start = false;
        time = 0;
        value.text = time.ToString("0: 00");
    }

    void Update()
    {
        if (start == true)
        {
            time -= Time.deltaTime;
            value.text = time.ToString("0: 00");
        }
    }
}