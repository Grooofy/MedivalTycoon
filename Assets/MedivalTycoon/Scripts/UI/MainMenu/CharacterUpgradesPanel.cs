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
        private int _selected;

        private void OnEnable()
        {
            Localization.LocalizationManager.LanguageChanged += Refresh;
            Refresh();
        }

        private void OnDisable() => Localization.LocalizationManager.LanguageChanged -= Refresh;

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
            _balance.text = Localization.LocalizationManager.Format("upgrade.Balance", LevelRewards.Balance);
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
                    string nextMugs = max ? string.Empty : Localization.LocalizationManager.Format("upgrade.TavernNextMugs", TavernUpgrades.GetValue(stat, level + 1));
                    view.Description.text = stat == TavernUpgrades.Stat.FillSpeed
                        ? Localization.LocalizationManager.Format("upgrade.TavernSpeed", level, maxLevel)
                        : Localization.LocalizationManager.Format("upgrade.TavernMugs", TavernUpgrades.BonusMugs, nextMugs, level, maxLevel);
                    view.Price.text = max ? Localization.LocalizationManager.Get("upgrade.Maximum") : Localization.LocalizationManager.Format("upgrade.Buy", TavernUpgrades.Price(stat));
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
                string title = view.Stat == CharacterUpgrades.Stat.Speed ? Localization.LocalizationManager.Get("upgrade.WorkerSpeed") : Localization.LocalizationManager.Get("upgrade.WorkerCapacity");
                view.Description.text = Localization.LocalizationManager.Format("upgrade.WorkerLevel", title, level, steps.Length);
                if (view.Stat == CharacterUpgrades.Stat.Capacity)
                {
                    float value = CharacterUpgrades.GetValue(worker, view.Stat, level);
                    string next = max ? string.Empty : Localization.LocalizationManager.Format("upgrade.WorkerNextValue", CharacterUpgrades.GetValue(worker, view.Stat, level + 1));
                    view.Description.text += Localization.LocalizationManager.Format("upgrade.WorkerCapacityValue", value, next);
                }
                view.Price.text = max ? Localization.LocalizationManager.Get("upgrade.Maximum") : Localization.LocalizationManager.Format("upgrade.Buy", steps[level].Price);
                view.Buy.interactable = !max && LevelRewards.Balance >= steps[level].Price;
            }
        }
    }
}
