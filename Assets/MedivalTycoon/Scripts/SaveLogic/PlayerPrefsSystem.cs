using UnityEngine;

public class PlayerPrefsSystem: ISaveSystem
{
    private const string LEVELKEY = "levelNumber";
    private const string MONEYKEY = "startMoney";
    private const string VISITORSKEY = "numberVisitors";
    private const string SECONDSKEY = "seconds";
    private const string TABLESAMOUNTKEY = "tableAmount";
    private const string TABLESCOSTKEY = "tableCost";
    private const string TUTORIALKEY = "isTutorialCompleted";

    public void Save(SaveData data)
    {
        PlayerPrefs.SetInt(LEVELKEY, data.NumberLevel);
        PlayerPrefs.SetInt(MONEYKEY, data.StartMoney);
        PlayerPrefs.SetInt(VISITORSKEY, data.NumberVisitors);
        PlayerPrefs.SetFloat(SECONDSKEY, data.Seconds);
        PlayerPrefs.SetInt(TABLESAMOUNTKEY, data.TableAmount);
        PlayerPrefs.SetString(TABLESCOSTKEY, data.TableCost); 
        PlayerPrefs.SetInt(TUTORIALKEY, data.IsTutorialCompleted ? 1 : 0);
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
        if (PlayerPrefs.HasKey(TABLESAMOUNTKEY))
        {
            result.TableAmount = PlayerPrefs.GetInt(TABLESAMOUNTKEY);
        }
        if (PlayerPrefs.HasKey(TABLESCOSTKEY))
        {
            result.TableCost = PlayerPrefs.GetString(TABLESCOSTKEY);
        }
        if (PlayerPrefs.HasKey(TUTORIALKEY))
        {
            result.IsTutorialCompleted = PlayerPrefs.GetInt(TUTORIALKEY) == 1;
        }
        return result;
    }
}