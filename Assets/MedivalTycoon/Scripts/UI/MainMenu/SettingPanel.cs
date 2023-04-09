using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour, IPanel
{
    [SerializeField] private float _durationAnimation;
    [SerializeField] private Button _ok;
    private Vector3 _startScale = Vector3.zero;
    private Vector3 _finishScale = Vector3.one;

    private void OnEnable()
    {
        _ok.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _ok.onClick.RemoveListener(Close);
    }

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