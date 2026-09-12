using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GamePauseButton : MonoBehaviour
{
    [SerializeField] private GameObject _pauseOverlay;
    [SerializeField] private RectTransform _sceneReloader;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Sprite _rewardedAdIcon;
    [SerializeField] private UnityEngine.Events.UnityEvent _onRewardedAdRequested = new UnityEngine.Events.UnityEvent();
    [SerializeField] private float _panelDuration = 0.5f;

    private UIAnimation _uiAnimations;
    private Button _pauseButton;
    private Vector2 _panelPosition;
    private Tween _panelTween;
    private float _previousTimeScale;
    private bool _isPaused;
    private bool _isClosing;
    private bool _isTimeOut;
    private GameObject _exitConfirmation;
    private Button _cancelExitButton;
    private Button _confirmExitButton;

    public void Initialize(UIAnimation uIAnimation)
    {
        _pauseButton = GetComponent<Button>();
        _uiAnimations = uIAnimation;
        _pauseButton.onClick.RemoveListener(PressButton);
        _pauseButton.onClick.AddListener(PressButton);
        _panelPosition = _sceneReloader.anchoredPosition;
        _pauseOverlay.SetActive(false);
        _resumeButton.onClick.RemoveListener(Resume);
        _resumeButton.onClick.AddListener(Resume);
        _restartButton.onClick.RemoveListener(RestartLevel);
        _restartButton.onClick.AddListener(RestartLevel);
        _exitButton.onClick.RemoveListener(ShowExitConfirmation);
        _exitButton.onClick.AddListener(ShowExitConfirmation);
        CreateExitConfirmation();
    }

    public void PressButton()
    {
        if (_isPaused) return;
        OpenPanel();
    }

    public void ShowTimeOut()
    {
        if (_isTimeOut) return;
        _isTimeOut = true;
        _pauseOverlay.name = "TimeOut";
        _resumeButton.gameObject.name = "RewardedAdButton";
        var icon = _resumeButton.GetComponent<Image>();
        icon.sprite = _rewardedAdIcon;
        icon.preserveAspect = true;
        _resumeButton.onClick.RemoveListener(Resume);
        _resumeButton.onClick.AddListener(RequestRewardedAd);
        OpenPanel();
    }

    private void RequestRewardedAd()
    {
        if (!_isTimeOut || _isClosing || _exitConfirmation.activeSelf) return;
        // The ad provider will grant extra time only after a completed rewarded view.
        _onRewardedAdRequested.Invoke();
    }

    private void OpenPanel()
    {
        _isPaused = true;
        _previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        _pauseButton.interactable = false;
        _uiAnimations.Lift();
        _pauseOverlay.SetActive(true);
        Canvas.ForceUpdateCanvases();
        _sceneReloader.anchoredPosition = HiddenPanelPosition();
        _panelTween = _sceneReloader.DOAnchorPos(_panelPosition, _panelDuration)
            .SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void Resume()
    {
        if (!_isPaused || _isClosing || _isTimeOut || _exitConfirmation.activeSelf) return;
        _isClosing = true;
        _panelTween?.Kill();
        _uiAnimations.Play();
        _panelTween = _sceneReloader.DOAnchorPos(HiddenPanelPosition(), _panelDuration)
            .SetEase(Ease.InSine).SetUpdate(true).OnComplete(() =>
            {
                _pauseOverlay.SetActive(false);
                Time.timeScale = _previousTimeScale;
                _isPaused = false;
                _isClosing = false;
                _pauseButton.interactable = true;
            });
    }

    public void RestartLevel()
    {
        if (!_isPaused || _isClosing || _exitConfirmation.activeSelf) return;
        _isClosing = true;
        _resumeButton.interactable = false;
        _restartButton.interactable = false;
        _panelTween?.Kill();
        _isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowExitConfirmation()
    {
        if (!_isPaused || _isClosing || _exitConfirmation.activeSelf) return;
        SetPauseButtonsInteractable(false);
        _exitConfirmation.SetActive(true);
        _exitConfirmation.transform.SetAsLastSibling();
        _cancelExitButton.Select();
    }

    public void CancelExit()
    {
        if (_isClosing) return;
        _exitConfirmation.SetActive(false);
        SetPauseButtonsInteractable(true);
        _exitButton.Select();
    }

    public void ConfirmExit()
    {
        if (!_isPaused || _isClosing || !_exitConfirmation.activeSelf) return;
        _isClosing = true;
        _confirmExitButton.interactable = false;
        _cancelExitButton.interactable = false;
        _panelTween?.Kill();
        _isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void SetPauseButtonsInteractable(bool value)
    {
        _resumeButton.interactable = value;
        _restartButton.interactable = value;
        _exitButton.interactable = value;
    }

    private void CreateExitConfirmation()
    {
        if (_exitConfirmation != null) return;
        var overlay = CreatePanel("ExitConfirmation", _pauseOverlay.transform,
            Vector2.zero, Vector2.one, new Color(0f, 0f, 0f, 0.75f));
        _exitConfirmation = overlay.gameObject;
        var panel = CreatePanel("Dialog", overlay, new Vector2(0.2f, 0.3f),
            new Vector2(0.8f, 0.7f), new Color(0.16f, 0.11f, 0.07f));
        CreateLabel("Title", panel, "Выйти в главное меню?", new Vector2(0.05f, 0.66f),
            new Vector2(0.95f, 0.94f), 38);
        CreateLabel("Warning", panel, "Вы уверены, что хотите выйти?\nПрогресс текущего уровня не сохранится.",
            new Vector2(0.06f, 0.35f), new Vector2(0.94f, 0.67f), 28);
        _cancelExitButton = CreateDialogButton("Cancel", panel, "Нет", 0.08f, 0.46f);
        _confirmExitButton = CreateDialogButton("Confirm", panel, "Да", 0.54f, 0.92f);
        _cancelExitButton.onClick.AddListener(CancelExit);
        _confirmExitButton.onClick.AddListener(ConfirmExit);
        _exitConfirmation.SetActive(false);
    }

    private static RectTransform CreatePanel(string name, Transform parent, Vector2 min, Vector2 max, Color color)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        obj.layer = parent.gameObject.layer;
        var rect = (RectTransform)obj.transform;
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        obj.GetComponent<Image>().color = color;
        return rect;
    }

    private static void CreateLabel(string name, Transform parent, string value, Vector2 min, Vector2 max, int size)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        obj.layer = parent.gameObject.layer;
        var rect = (RectTransform)obj.transform;
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        var label = obj.GetComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.text = value;
        label.fontSize = size;
        label.resizeTextForBestFit = true;
        label.resizeTextMinSize = 12;
        label.resizeTextMaxSize = size;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = new Color(1f, 0.94f, 0.8f);
        label.raycastTarget = false;
    }

    private static Button CreateDialogButton(string name, Transform parent, string label, float left, float right)
    {
        var rect = CreatePanel(name, parent, new Vector2(left, 0.08f), new Vector2(right, 0.29f),
            new Color(0.42f, 0.29f, 0.13f));
        var button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = rect.GetComponent<Image>();
        CreateLabel("Label", rect, label, Vector2.zero, Vector2.one, 30);
        return button;
    }

    private Vector2 HiddenPanelPosition()
    {
        var parent = (RectTransform)_sceneReloader.parent;
        return _panelPosition + Vector2.up * (parent.rect.height + _sceneReloader.rect.height);
    }

    private void OnDestroy()
    {
        _panelTween?.Kill();
        if (_pauseButton != null) _pauseButton.onClick.RemoveListener(PressButton);
        if (_resumeButton != null) _resumeButton.onClick.RemoveListener(Resume);
        if (_resumeButton != null) _resumeButton.onClick.RemoveListener(RequestRewardedAd);
        if (_restartButton != null) _restartButton.onClick.RemoveListener(RestartLevel);
        if (_exitButton != null) _exitButton.onClick.RemoveListener(ShowExitConfirmation);
        if (_cancelExitButton != null) _cancelExitButton.onClick.RemoveListener(CancelExit);
        if (_confirmExitButton != null) _confirmExitButton.onClick.RemoveListener(ConfirmExit);
        if (_isPaused) Time.timeScale = _previousTimeScale;
    }

}
