using System;
using Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(ViewButton), typeof(ModelButton))]
public class ManagerButton : MonoBehaviour
{
    private Button _button;
    private ViewButton _view;
    private ModelButton _model;

    public UnityAction<int> ReceivingPressedButtonId;

    public void Initilaze(Button button, ViewButton view, ModelButton model, Image image, Worker worker)
    {
        _view = view;
        _view.Initialize(image);
        _model = model;
        _model.Initialize(worker);
        _button = button;
        _button.onClick.AddListener(ClickButton);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(ClickButton);
    }

    public void ShowInformation()
    {
        _view.ShowInfoObject(_button, _model.GetStatus(), _model.GetIcon());
    }

    public bool GetButtonStatus()
    {
        return _model.GetStatus();
    }

    public void RefreshButton()
    {
        _model.SelectWorker();
        _view.ShowInfoObject(_button, _model.GetStatus(), _model.GetIcon());
    }

    private void ClickButton()
    {
        RefreshButton();
        ReceivingPressedButtonId?.Invoke(_model.GetId());
    }

}