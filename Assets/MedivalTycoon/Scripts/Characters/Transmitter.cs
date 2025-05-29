using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transmitter : MonoBehaviour
{
    [SerializeField] private List<ManagerButton> _buttons;

    public UnityAction<int> CharacterSelected;
    private ManagerButton _previousButton;
    private ManagerButton _firstButton;


    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.ReceivingPressedButtonId += GiveSignal;
        }
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.ReceivingPressedButtonId -= GiveSignal;
        }
    }

    private void Awake()
    {
        _firstButton = _buttons[0];
    }

    private void GiveSignal(int id)
    {
        _previousButton = _firstButton;
        _previousButton.RefreshButton();
        _firstButton = _buttons[id];
        CharacterSelected?.Invoke(id);
    }
}