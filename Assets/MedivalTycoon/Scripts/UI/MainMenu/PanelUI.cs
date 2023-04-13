using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelUI: MonoBehaviour
{
    [SerializeField] private float _durationAnimation;
    private Vector3 _startScale = Vector3.zero;
    private Vector3 _finishScale = Vector3.one;
    
    public virtual void Open()
    {
        transform.DOScale(_finishScale, _durationAnimation);
    }

    public virtual void Close()
    {
        transform.DOScale(_startScale, _durationAnimation);
    }
}