using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(ViewButton), typeof(ModelButton))]
public class ManagerButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private ViewButton _view;
    [SerializeField] private ModelButton _model;

    public UnityAction<int> ReceivingPressedButtonId;
    
    private void OnEnable()
    {
        _button.onClick.AddListener(ClickButton);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ClickButton);
    }

    private void Awake()
    {
        _view.ShowInfoObject(_button, _model.GetStatus(), _model.GetIcon());
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
