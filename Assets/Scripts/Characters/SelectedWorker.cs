using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWorker : MonoBehaviour
{
    [SerializeField] private Worker[] _workers;

    private Button[] _buttons;
    private Image _image;
    private int indexSelectWorker;

    private void OnEnable()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].onClick.AddListener(() =>ClickButton(i));
           // _workers[i].Selected += ShowWorker(i);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].onClick.RemoveListener(() =>ClickButton(i));
            //_workers[i].Selected -= ShowWorker(i);
        }
    }

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();
        
        for (int i = 0; i < _buttons.Length; i++)
        {
            _image = _buttons[i].GetComponent<Image>();
            ShowWorker(i);
        }
    }

    private void ShowWorker(int index)
    {
        _image.sprite = _workers[index].Icon;
        _buttons[index].interactable = !_workers[index].Select;
    }

    private void ClickButton(int index)
    {
        _workers[index].ChangeValueSelect();
    }
}
