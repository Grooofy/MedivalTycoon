
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GamePauseButton _pauseButton;
    [SerializeField] private UIAnimation _uiAnimations;


    public void Initialize()
    {
        _uiAnimations.Initialize();
        _pauseButton.Initialize(_uiAnimations);
    }
}
