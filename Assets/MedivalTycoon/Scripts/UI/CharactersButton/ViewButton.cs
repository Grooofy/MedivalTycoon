using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image _image;
    private Vector3 _originalScale;
  
    public void Initialize(Image image)
    {
        if (image == null)
        {
            Debug.LogError($"Image {image.name} is null");
        }
        _image = image;
        _originalScale = transform.localScale;
    }

    public void ShowInfoObject(Button button, bool isValue, Sprite icon)
    {
        _image.sprite = icon;
        button.interactable = !isValue;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_originalScale, 0.2f).SetEase(Ease.InOutSine);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOComplete(); 
        transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1f);
    }
}