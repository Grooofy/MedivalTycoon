using UnityEngine;

public static class LevelRewards
{
    private const string BalanceKey = "Progress.Coins";
    private static string BestKey(int level) => "Progress.Level." + level + ".Coins";

    public static int Balance => PlayerPrefs.GetInt(BalanceKey, 0);
    public static int GetBest(int level) => PlayerPrefs.GetInt(BestKey(level), 0);

    public static int Calculate(float remainingSeconds, float totalSeconds)
    {
        if (remainingSeconds <= 0f || totalSeconds <= 0f) return 0;
        float fraction = remainingSeconds / totalSeconds;
        return fraction >= 0.5f ? 3 : fraction >= 0.25f ? 2 : 1;
    }

    public static int Grant(int level, int result)
    {
        result = Mathf.Clamp(result, 0, 3);
        int earned = Mathf.Max(0, result - GetBest(level));
        if (earned == 0) return 0;
        PlayerPrefs.SetInt(BestKey(level), result);
        PlayerPrefs.SetInt(BalanceKey, Balance + earned);
        PlayerPrefs.Save();
        return earned;
    }
}
