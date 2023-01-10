using UnityEngine;
using UnityEngine.UI;

public class ViewButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private ActiveCharacter _activeCharacter;

    public void ShowInfoObject(Button button, bool isValue, Sprite icon)
    {
        _image.sprite = icon;
        button.interactable = !isValue;
    }
}
