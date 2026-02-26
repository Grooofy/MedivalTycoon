using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class TutorialSpotlight : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Color overlayColor = new Color(0, 0, 0, 0.8f);
    [SerializeField] private float padding = 20f;
    [SerializeField] private float moveDuration = 0.5f;

    [SerializeField] private Canvas _canvas;
    private List<Image> _panels = new List<Image>();
    private RectTransform _targetRect;
    
    // Поля для анимации
    private Tween _moveTween;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private Vector2 _currentAnimPos; // Позиция в текущем кадре анимации
    private Vector2 _startSize;
    private Vector2 _endSize;
    private Vector2 _currentAnimSize;

    private void Awake()
    {
        if (_canvas == null)
        {
            // Попытка найти автоматически, если не назначен
            _canvas = FindObjectOfType<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("Canvas not found!");
                return;
            }
        }

        CreateOverlayPanels();
        HideSpotlight();
    }

    private void CreateOverlayPanels()
    {
        string[] names = { "Top", "Bottom", "Left", "Right" };
        foreach (var name in names)
        {
            GameObject panelObj = new GameObject($"Spotlight_{name}");
            panelObj.transform.SetParent(_canvas.transform, false);

            Image img = panelObj.AddComponent<Image>();
            img.color = overlayColor;
            img.raycastTarget = true;

            RectTransform rect = panelObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);

            _panels.Add(img);
        }
        HideSpotlight();
    }

    public void ShowSpotlight(RectTransform target)
    {
        _targetRect = target;
        gameObject.SetActive(true);
        
        // Инициализируем позиции сразу
        _currentAnimPos = GetTargetScreenPosition();
        _currentAnimSize = GetHoleSize();
        
        UpdatePanelsLayout();
    }

    public void MoveSpotlight(RectTransform target)
    {
        if (_targetRect == null)
        {
            ShowSpotlight(target);
            return;
        }

        _targetRect = target;

        if (_moveTween != null && _moveTween.IsActive())
            _moveTween.Kill();

        // Запоминаем старт и конец
        _startPos = _currentAnimPos;
        _endPos = GetTargetScreenPosition();
        
        _startSize = _currentAnimSize;
        _endSize = GetHoleSize();

        // Анимируем фиктивную переменную от 0 до 1
        _moveTween = DOTween.To(
            () => 0f, 
            (t) => OnAnimationUpdate(t), // t меняется от 0 до 1
            1f, 
            moveDuration
        ).SetEase(Ease.OutQuad);
    }

    // Этот метод вызывает DOTween каждый кадр
    private void OnAnimationUpdate(float t)
    {
        // Интерполяция позиций и размеров
        _currentAnimPos = Vector2.Lerp(_startPos, _endPos, t);
        _currentAnimSize = Vector2.Lerp(_startSize, _endSize, t);
        
        UpdatePanelsLayout();
    }

    public void HideSpotlight()
    {
        if (_moveTween != null) _moveTween.Kill();
        gameObject.SetActive(false);
    }

    private void UpdatePanelsLayout()
    {
        if (_targetRect == null) return;

        // Используем анимированные значения, если они есть, иначе считаем сразу
        Vector2 targetPos = (_moveTween != null && _moveTween.IsActive()) 
            ? _currentAnimPos 
            : GetTargetScreenPosition();
            
        Vector2 holeSize = (_moveTween != null && _moveTween.IsActive()) 
            ? _currentAnimSize 
            : GetHoleSize();

        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float leftX = targetPos.x - holeSize.x / 2;
        float rightX = targetPos.x + holeSize.x / 2;
        float bottomY = targetPos.y - holeSize.y / 2;
        float topY = targetPos.y + holeSize.y / 2;

        SetPanelRect(_panels[0], 0, topY, canvasWidth, canvasHeight - topY, 0.5f, 0);
        SetPanelRect(_panels[1], 0, 0, canvasWidth, bottomY, 0.5f, 1);
        SetPanelRect(_panels[2], 0, bottomY, leftX, holeSize.y, 0, 0.5f);
        SetPanelRect(_panels[3], rightX, bottomY, canvasWidth - rightX, holeSize.y, 1, 0.5f);
    }

    private void SetPanelRect(Image panel, float x, float y, float w, float h, float pivotX, float pivotY)
    {
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.pivot = new Vector2(pivotX, pivotY);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(w, h);
        rect.anchoredPosition = new Vector2(x + w * pivotX, y + h * pivotY);
    }

    private Vector2 GetTargetScreenPosition()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.GetComponent<RectTransform>(),
            _targetRect.position,
            null,
            out localPoint
        );
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        return new Vector2(
            localPoint.x + canvasRect.rect.width / 2,
            localPoint.y + canvasRect.rect.height / 2
        );
    }

    private Vector2 GetHoleSize()
    {
        if (_targetRect == null) return Vector2.one * 100f;
        return _targetRect.rect.size + Vector2.one * padding * 2;
    }
}