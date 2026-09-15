using UnityEngine;

public static class TavernUpgrades
{
    public enum Stat { FillSpeed, MugsPerBarrel }
    private static TavernSettings _settings;
    public static TavernSettings Settings
    {
        get
        {
            if (_settings == null) _settings = Resources.Load<TavernSettings>("TavernSettings");
            if (_settings == null)
                throw new System.InvalidOperationException("Missing Resources/TavernSettings.asset");
            return _settings;
        }
    }
    public static int GetMaxLevel(Stat stat) => Settings.GetMaxLevel(stat);
    public static int GetLevel(Stat stat) => Mathf.Clamp(PlayerPrefs.GetInt("Progress.Tavern." + stat, 0), 0, GetMaxLevel(stat));
    public static int Price(Stat stat) => GetLevel(stat) < GetMaxLevel(stat) ? Settings.GetPrice(stat, GetLevel(stat)) : 0;
    public static float GetValue(Stat stat, int level) => Settings.GetValue(stat, level);
    public static float SpeedMultiplier => GetValue(Stat.FillSpeed, GetLevel(Stat.FillSpeed));
    public static int BonusMugs => Mathf.RoundToInt(GetValue(Stat.MugsPerBarrel, GetLevel(Stat.MugsPerBarrel)));
    public static bool CanBuy(Stat stat) => GetLevel(stat) < GetMaxLevel(stat) &&
        Settings.IsValidStep(stat, GetLevel(stat)) && LevelRewards.Balance >= Price(stat);
    public static float FillSeconds(float seconds) => Mathf.Max(0.01f, seconds) / SpeedMultiplier;
    public static bool TryBuy(Stat stat)
    {
        if (stat != Stat.FillSpeed && stat != Stat.MugsPerBarrel) return false;
        if (!CanBuy(stat) || !LevelRewards.TrySpend(Price(stat))) return false;
        PlayerPrefs.SetInt("Progress.Tavern." + stat, GetLevel(stat) + 1);
        PlayerPrefs.Save();
        return true;
    }
}
