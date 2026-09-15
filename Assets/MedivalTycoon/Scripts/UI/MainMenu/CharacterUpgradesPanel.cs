using System;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public sealed class CharacterUpgradesPanel : PanelUI
    {
        [Serializable]
        private sealed class UpgradeLabels
        {
            [Tooltip("{0}: balance")]
            public string Balance = "Монеты: {0}";
            [Tooltip("{0}: price")]
            public string Buy = "Улучшить · {0}";
            public string Maximum = "Максимум";
            public string WorkerSpeed = "Скорость";
            public string WorkerCapacity = "Вместимость";
            [Tooltip("{0}: title, {1}: purchased level, {2}: maximum level")]
            public string WorkerLevel = "{0} {1}/{2}";
            [TextArea, Tooltip("{0}: current value, {1}: next value text")]
            public string WorkerCapacityValue = "\n{0:0.##}{1}";
            [Tooltip("{0}: next value")]
            public string WorkerNextValue = " → {0:0.##}";
            [TextArea, Tooltip("{0}: purchased level, {1}: maximum level")]
            public string TavernSpeed = "Скорость получения бочек\nУровень: {0}";
            [TextArea, Tooltip("{0}: bonus mugs, {1}: next value text, {2}: purchased level, {3}: maximum level")]
            public string TavernMugs = "Кружек из бочки +{0}{1}\n{2}/{3}";
            [Tooltip("{0}: next bonus mugs")]
            public string TavernNextMugs = " → +{0:0}";
        }

        [Serializable]
        private struct WorkerChoice
        {
            public Worker Worker;
            public Button Button;
            public Image Icon;
        }

        [Serializable]
        private struct UpgradeView
        {
            public CharacterUpgrades.Stat Stat;
            public TMP_Text Description;
            public TMP_Text Price;
            public Button Buy;
        }

        [SerializeField] private WorkerChoice[] _workers;
        [SerializeField] private UpgradeView[] _upgrades;
        [SerializeField] private TMP_Text _balance;
        [SerializeField] private Button _close;
        [SerializeField] private Button _tavern;
        [SerializeField] private UpgradeLabels _labels = new UpgradeLabels();
        private int _selected;

        private void Awake()
        {
            if (_tavern != null) _tavern.onClick.AddListener(() => SelectWorker(-1));
            for (int i = 0; i < _workers.Length; i++)
            {
                int index = i;
                _workers[i].Icon.sprite = _workers[i].Worker.Icon;
                _workers[i].Button.onClick.AddListener(() => SelectWorker(index));
            }
            foreach (var view in _upgrades)
            {
                var stat = view.Stat;
                view.Buy.onClick.AddListener(() => Buy(stat));
            }
            _close.onClick.AddListener(Close);
        }

        public override void Open()
        {
            Refresh();
            base.Open();
        }

        private void SelectWorker(int index)
        {
            _selected = index;
            Refresh();
        }

        private void Buy(CharacterUpgrades.Stat stat)
        {
            if (_selected < 0) TavernUpgrades.TryBuy((TavernUpgrades.Stat)stat);
            else CharacterUpgrades.TryBuy(_workers[_selected].Worker, stat);
            Refresh();
        }

        public void Refresh()
        {
            _balance.text = string.Format(_labels.Balance, LevelRewards.Balance);
            if (_tavern != null) _tavern.interactable = _selected >= 0;
            for (int i = 0; i < _workers.Length; i++)
            {
                _workers[i].Button.interactable = i != _selected;
                var color = _workers[i].Icon.color;
                color.a = i == _selected ? 0.5f : 1f;
                _workers[i].Icon.color = color;
            }
            if (_selected < 0)
            {
                foreach (var view in _upgrades)
                {
                    var stat = (TavernUpgrades.Stat)view.Stat;
                    int level = TavernUpgrades.GetLevel(stat);
                    int maxLevel = TavernUpgrades.GetMaxLevel(stat);
                    bool max = level >= maxLevel;
                    string nextMugs = max ? string.Empty : string.Format(_labels.TavernNextMugs, TavernUpgrades.GetValue(stat, level + 1));
                    view.Description.text = stat == TavernUpgrades.Stat.FillSpeed
                        ? string.Format(_labels.TavernSpeed, level, maxLevel)
                        : string.Format(_labels.TavernMugs, TavernUpgrades.BonusMugs, nextMugs, level, maxLevel);
                    view.Price.text = max ? _labels.Maximum : string.Format(_labels.Buy, TavernUpgrades.Price(stat));
                    view.Buy.interactable = TavernUpgrades.CanBuy(stat);
                }
                return;
            }
            var worker = _workers[_selected].Worker;
            foreach (var view in _upgrades)
            {
                int level = CharacterUpgrades.GetLevel(worker, view.Stat);
                var steps = worker.GetUpgrades(view.Stat);
                bool max = level >= steps.Length;
                string title = view.Stat == CharacterUpgrades.Stat.Speed ? _labels.WorkerSpeed : _labels.WorkerCapacity;
                view.Description.text = string.Format(_labels.WorkerLevel, title, level, steps.Length);
                if (view.Stat == CharacterUpgrades.Stat.Capacity)
                {
                    float value = CharacterUpgrades.GetValue(worker, view.Stat, level);
                    string next = max ? string.Empty : string.Format(_labels.WorkerNextValue, CharacterUpgrades.GetValue(worker, view.Stat, level + 1));
                    view.Description.text += string.Format(_labels.WorkerCapacityValue, value, next);
                }
                view.Price.text = max ? _labels.Maximum : string.Format(_labels.Buy, steps[level].Price);
                view.Buy.interactable = !max && LevelRewards.Balance >= steps[level].Price;
            }
        }
    }
}
