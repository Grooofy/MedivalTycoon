using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWorker : MonoBehaviour
{
    [SerializeField] private Worker[] _workers;

    private Button[] _buttons;
    private Image _image;
    private int index;

    private void OnEnable()
    {
        for (int index = 0; index < _buttons.Length; index++)
        {
            _buttons[index].onClick.AddListener(ClickButton);
            _workers[index].Selected += ShowWorker;
        }
    }

    private void OnDisable()
    {
        for (int index = 0; index < _buttons.Length; index++)
        {
            _buttons[index].onClick.RemoveListener(ClickButton);
            _workers[index].Selected -= ShowWorker;
        }
    }

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        
        for (int index = 0; index < _buttons.Length; index++)
        {
            _image = _buttons[index].GetComponent<Image>();
            ShowWorker();
        }
    }

    private void ShowWorker()
    {
        _image.sprite = _workers[index].Icon;
        _buttons[index].interactable = !_workers[index].Select;
    }

    private void ClickButton()
    {
        _workers[index].ChangeValueSelect();
    }
}
