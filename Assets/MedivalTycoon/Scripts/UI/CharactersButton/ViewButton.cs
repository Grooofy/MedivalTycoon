using System;
using UnityEngine;
using UnityEngine.UI;

public class ViewButton : MonoBehaviour
{
    private Image _image;
  
    public void Initialize(Image image)
    {
        if (image == null)
        {
            Debug.LogError($"Image {image.name} is null");
        }
        _image = image;
    }

    public void ShowInfoObject(Button button, bool isValue, Sprite icon)
    {
        _image.sprite = icon;
        button.interactable = !isValue;
    }
}