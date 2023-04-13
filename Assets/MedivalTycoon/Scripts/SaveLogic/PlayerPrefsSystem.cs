using UnityEngine;

public class PlayerPrefsSystem: ISaveSystem
{
    private const string LEVELKEY = "levelNumber";
    private const string MONEYKEY = "startMoney";
    private const string VISITORSKEY = "numberVisitors";
    private const string SECONDSKEY = "seconds";

    public void Save(SaveData data)
    {
        PlayerPrefs.SetInt(LEVELKEY, data.NumberLevel);
        PlayerPrefs.SetInt(MONEYKEY, data.StartMoney);
        PlayerPrefs.SetInt(VISITORSKEY, data.NumberVisitors);
        PlayerPrefs.SetFloat(SECONDSKEY, data.Seconds);
        PlayerPrefs.Save();
    }

    public SaveData Load()
    {
        var result = new SaveData();
        
        if (PlayerPrefs.HasKey(LEVELKEY))
        {
            result.NumberLevel = PlayerPrefs.GetInt(LEVELKEY);
        }
        if (PlayerPrefs.HasKey(MONEYKEY))
        {
            result.StartMoney = PlayerPrefs.GetInt(MONEYKEY);
        }
        if (PlayerPrefs.HasKey(VISITORSKEY))
        {
            result.NumberVisitors = PlayerPrefs.GetInt(VISITORSKEY);
        }
        if (PlayerPrefs.HasKey(SECONDSKEY))
        {
            result.Seconds = PlayerPrefs.GetFloat(SECONDSKEY);
        }
        return result;
    }
}