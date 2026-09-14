using UnityEngine;

namespace Characters
{
    public static class CharacterUpgrades
    {
        public enum Stat { Speed, Capacity }
        private static string Key(Worker worker, Stat stat) => $"Progress.Worker.{worker.Id}.{stat}";

        public static int GetLevel(Worker worker, Stat stat) =>
            Mathf.Clamp(PlayerPrefs.GetInt(Key(worker, stat), 0), 0, worker.GetUpgrades(stat).Length);

        public static float GetValue(Worker worker, Stat stat, int level)
        {
            float value = stat == Stat.Speed ? worker.MoveSpeed : worker.NumberWearableObjects;
            var steps = worker.GetUpgrades(stat);
            for (int i = 0; i < Mathf.Clamp(level, 0, steps.Length); i++)
                value += stat == Stat.Capacity ? Mathf.RoundToInt(steps[i].Increase) : steps[i].Increase;
            return value;
        }

        public static float GetValue(Worker worker, Stat stat) => GetValue(worker, stat, GetLevel(worker, stat));

        public static bool TryBuy(Worker worker, Stat stat)
        {
            if (worker == null || (stat != Stat.Speed && stat != Stat.Capacity)) return false;
            int level = GetLevel(worker, stat);
            var steps = worker.GetUpgrades(stat);
            if (level == steps.Length || steps[level].Increase <= 0f ||
                (stat == Stat.Capacity && Mathf.RoundToInt(steps[level].Increase) < 1)) return false;
            if (!LevelRewards.TrySpend(steps[level].Price)) return false;
            PlayerPrefs.SetInt(Key(worker, stat), level + 1);
            PlayerPrefs.Save();
            return true;
        }
    }
}
