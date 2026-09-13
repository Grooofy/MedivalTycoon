using UnityEngine;
using DG.Tweening;

public class PanelUI: MonoBehaviour
{
    [SerializeField] private float _durationAnimation;
    [SerializeField] private CanvasGroup _canvasGroup;

    private int _maxValue = 1;
    private int _minValue = 0;

    public virtual void Open()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(_maxValue, _durationAnimation);
        SwitchCanvasGroup(true);
    }

    public virtual void Close()
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(_minValue, _durationAnimation);
        SwitchCanvasGroup(false);
    }

    public void HideImmediately()
    {
        _canvasGroup.DOKill();
        _canvasGroup.alpha = 0f;
        SwitchCanvasGroup(false);
    }

    private void SwitchCanvasGroup(bool value)
    {
        _canvasGroup.interactable = value;
        _canvasGroup.blocksRaycasts = value;
    }
}
