using UnityEngine;
using UnityEngine.UI;

public class GamePauseButton : MonoBehaviour
{
    private UIAnimation _uiAnimations;
    private Button _pauseButton;

    public void Initialize(UIAnimation uIAnimation)
    {
        _pauseButton = GetComponent<Button>();
        _uiAnimations = uIAnimation;
        _pauseButton.onClick.AddListener(PressButton);
    }

    public void PressButton()
    {
        _uiAnimations.Lift();
        Time.timeScale = 0; 
    }


}
