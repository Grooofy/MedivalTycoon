using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Characters
{
    public class ButtonsTransmitter : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons = new List<Button>();
        [SerializeField] private List<ViewButton> _viewButtons = new List<ViewButton>();
        [SerializeField] private List<ModelButton> _modelButtons = new List<ModelButton>();
        [SerializeField] private List<Image> _images = new List<Image>();
        [SerializeField] private List<Worker> _workers = new List<Worker>();

        [SerializeField] private List<ManagerButton> _managerButtons;

        public UnityAction<int> CharacterSelected;
        private ManagerButton _previousButton;
        private ManagerButton _firstButton;


        private void OnEnable()
        {
            foreach (var button in _managerButtons)
            {
                button.ReceivingPressedButtonId += GiveSignal;
            }
        }

        private void OnDisable()
        {
            foreach (var button in _managerButtons)
            {
                button.ReceivingPressedButtonId -= GiveSignal;
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < _managerButtons.Count; i++)
            {
                _managerButtons[i].Initilaze(_buttons[i], _viewButtons[i], _modelButtons[i], _images[i], _workers[i]);
                _managerButtons[i].ShowInformation();

                if (_managerButtons[i].GetButtonStatus())
                {
                    _firstButton = _managerButtons[i];
                }
            }
        }

        private void GiveSignal(int id)
        {
            _previousButton = _firstButton;
            _previousButton.RefreshButton();
            _firstButton = _managerButtons[id];
            CharacterSelected?.Invoke(id);
        }
    }
}