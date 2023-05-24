using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _loadingImage;
   
    private static SceneTransition _instance;
    private static bool _shouldPlayOpenAnimation = false;
    
    private AsyncOperation _loadingSceneOperation;

    private void Awake()
    {
        _instance = this;
        
        if (_shouldPlayOpenAnimation)
        {
            _animator.SetTrigger("sceneEnd");
        }
    }

    private void Update()
    {
        if (_loadingSceneOperation != null)
        {
            _loadingImage.fillAmount = Mathf.Lerp(_loadingImage.fillAmount, _loadingSceneOperation.progress, Time.deltaTime *5) ;
        }
    }

    public static void SwitchToScene(string sceneName)
    {
        _instance._animator.SetTrigger("sceneStart");
        _instance._loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _instance._loadingSceneOperation.allowSceneActivation = false;
    }

    public void OnAnimationOver()
    {
        _shouldPlayOpenAnimation = true;
        _loadingSceneOperation.allowSceneActivation = true;
    }
    
    
}
