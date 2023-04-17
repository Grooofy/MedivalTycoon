using UnityEngine;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    private const string NAMESCENE = "Game";
    
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OpenGameScene);
    }

    private void OpenGameScene()
    {
        SceneTransition.SwitchToScene(NAMESCENE);
    }
}
