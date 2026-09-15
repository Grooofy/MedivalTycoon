using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TavernSettings", menuName = "Tavern/Upgrade Settings", order = 42)]
public sealed class TavernSettings : ScriptableObject
{
    [Serializable]
    public struct SpeedUpgrade
    {
        [Min(1)] public int Price;
        [Min(0.01f), Tooltip("Added speed percentage: 25 adds 25% of the base speed.")]
        public float IncreasePercent;
    }

    [Serializable]
    public struct MugUpgrade
    {
        [Min(1)] public int Price;
        [Min(1), Tooltip("Additional mugs per barrel at this step.")]
        public int Increase;
    }

    [SerializeField] private SpeedUpgrade[] _speedUpgrades = Array.Empty<SpeedUpgrade>();
    [SerializeField] private MugUpgrade[] _mugUpgrades = Array.Empty<MugUpgrade>();

    public int GetMaxLevel(TavernUpgrades.Stat stat) => stat == TavernUpgrades.Stat.FillSpeed
        ? (_speedUpgrades?.Length ?? 0) : (_mugUpgrades?.Length ?? 0);

    public int GetPrice(TavernUpgrades.Stat stat, int step) => stat == TavernUpgrades.Stat.FillSpeed
        ? Mathf.Max(1, _speedUpgrades[step].Price) : Mathf.Max(1, _mugUpgrades[step].Price);

    public float GetValue(TavernUpgrades.Stat stat, int level)
    {
        float value = stat == TavernUpgrades.Stat.FillSpeed ? 1f : 0f;
        for (int i = 0; i < Mathf.Clamp(level, 0, GetMaxLevel(stat)); i++)
            value += stat == TavernUpgrades.Stat.FillSpeed
                ? Mathf.Max(0f, _speedUpgrades[i].IncreasePercent) / 100f
                : Mathf.Max(0, _mugUpgrades[i].Increase);
        return value;
    }

    public bool IsValidStep(TavernUpgrades.Stat stat, int step) => stat == TavernUpgrades.Stat.FillSpeed
        ? _speedUpgrades[step].Price > 0 && _speedUpgrades[step].IncreasePercent > 0f
        : _mugUpgrades[step].Price > 0 && _mugUpgrades[step].Increase > 0;
}
