using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveCharacter : MonoBehaviour
{
    [SerializeField] private List<ManagerButton> _buttons;

    public UnityAction<int> CharacterSelected;
    private ManagerButton _previousButton;


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
        _previousButton = _buttons[0];
    }

    private void GiveSignal(int id)
    {
       _previousButton.RefreshButton();
       _previousButton = _buttons[id];
       CharacterSelected?.Invoke(id);
    }
}
