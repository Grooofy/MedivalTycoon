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
        private int _selected;

        private void Awake()
        {
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
            CharacterUpgrades.TryBuy(_workers[_selected].Worker, stat);
            Refresh();
        }

        private void Refresh()
        {
            _balance.text = $"Монеты: {LevelRewards.Balance}";
            for (int i = 0; i < _workers.Length; i++)
                _workers[i].Button.interactable = i != _selected;
            var worker = _workers[_selected].Worker;
            foreach (var view in _upgrades)
            {
                int level = CharacterUpgrades.GetLevel(worker, view.Stat);
                var steps = worker.GetUpgrades(view.Stat);
                bool max = level >= steps.Length;
                string title = view.Stat == CharacterUpgrades.Stat.Speed ? "Скорость" : "Вместимость";
                float value = CharacterUpgrades.GetValue(worker, view.Stat, level);
                string next = max ? "" : $" → {CharacterUpgrades.GetValue(worker, view.Stat, level + 1):0.##}";
                view.Description.text = $"{title} · {level}/{steps.Length}\n{value:0.##}{next}";
                view.Price.text = max ? "Максимум" : $"Улучшить · {steps[level].Price}";
                view.Buy.interactable = !max && LevelRewards.Balance >= steps[level].Price;
            }
        }
    }
}
