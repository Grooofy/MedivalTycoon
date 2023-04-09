using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour, IPanel
{
    [SerializeField] private Button _start;
    [SerializeField] private float _durationAnimation;
    private Vector3 _startScale = Vector3.zero;
    private Vector3 _finishScale = Vector3.one;
    
    public void Open()
    {
        this.gameObject.SetActive(true);
        transform.DOScale(_finishScale, _durationAnimation);
    }

    public void Close()
    {
        transform.DOScale(_startScale, _durationAnimation);
        this.gameObject.SetActive(false);
    }
}
