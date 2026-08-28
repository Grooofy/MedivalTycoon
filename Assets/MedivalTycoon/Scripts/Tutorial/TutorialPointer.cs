using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    // Показывает линию и стрелку от персонажа к целевой точке на Canvas
    public class TutorialPointer : MonoBehaviour
    {
        [SerializeField] private Sprite arrowSprite;
        [SerializeField] private Color lineColor = Color.yellow;
        [SerializeField] private float lineThickness = 6f;
        [SerializeField] private float arrowSize = 32f;

        private Canvas _canvas;
        private Image _lineImage;
        private Image _arrowImage;

        private Characters.ICharacter _fromCharacter;
        private Transform _toTransform;

        private RectTransform _lineRect;
        private RectTransform _arrowRect;

        public void Initialize(Canvas canvas)
        {
            _canvas = canvas;
            CreateUIElements();
            HidePointer();
        }

        private void CreateUIElements()
        {
            if (_canvas == null) return;
            Debug.Log("Creating TutorialPointer UI elements");  
            if (_lineImage == null)
            {
                GameObject lineObj = new GameObject("TutorialPointer_Line");
                lineObj.transform.SetParent(_canvas.transform, false);
                _lineImage = lineObj.AddComponent<Image>();
                _lineImage.color = lineColor;
                _lineRect = _lineImage.rectTransform;
                _lineRect.pivot = new Vector2(0.5f, 0.5f);
                _lineImage.raycastTarget = false;
            }

            if (_arrowImage == null)
            {
                GameObject arrowObj = new GameObject("TutorialPointer_Arrow");
                arrowObj.transform.SetParent(_canvas.transform, false);
                _arrowImage = arrowObj.AddComponent<Image>();
                _arrowImage.sprite = arrowSprite;
                _arrowImage.color = lineColor;
                _arrowRect = _arrowImage.rectTransform;
                _arrowRect.pivot = new Vector2(0.5f, 0.5f);
                _arrowImage.raycastTarget = false;
            }
        }

        public void ShowPointer(Characters.ICharacter fromCharacter, Transform to)
        {
            _fromCharacter = fromCharacter;
            _toTransform = to;
            if (_lineImage == null || _arrowImage == null) CreateUIElements();
            if (_lineImage != null) _lineImage.gameObject.SetActive(true);
            if (_arrowImage != null) _arrowImage.gameObject.SetActive(true);
            UpdatePointer();
        }

        public void HidePointer()
        {
            _fromCharacter = null;
            _toTransform = null;
            if (_lineImage != null) _lineImage.gameObject.SetActive(false);
            if (_arrowImage != null) _arrowImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_fromCharacter == null || _toTransform == null) return;
            UpdatePointer();
        }

        private void UpdatePointer()
        {
            if (_canvas == null || _lineRect == null || _arrowRect == null) return;

            Vector3 fromWorld = _fromCharacter != null ? _fromCharacter.GetPosition() : Vector3.zero;
            Vector3 toWorld = _toTransform.position;

            RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

            Vector2 fromScreen;
            Vector2 toScreen;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(fromWorld), _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera, out fromScreen);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(toWorld), _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera, out toScreen);

            Vector2 dir = toScreen - fromScreen;
            float dist = dir.magnitude;
            if (dist < 1f)
            {
                _lineRect.sizeDelta = Vector2.zero;
                _arrowRect.anchoredPosition = toScreen;
                return;
            }

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // Line: centered between points, rotated
            _lineRect.sizeDelta = new Vector2(dist, lineThickness);
            _lineRect.anchoredPosition = fromScreen + dir * 0.5f;
            _lineRect.localRotation = Quaternion.Euler(0, 0, angle);

            // Arrow: at end
            _arrowRect.sizeDelta = new Vector2(arrowSize, arrowSize);
            _arrowRect.anchoredPosition = toScreen;
            _arrowRect.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private void OnDestroy()
        {
            if (_lineImage != null) Destroy(_lineImage.gameObject);
            if (_arrowImage != null) Destroy(_arrowImage.gameObject);
        }
    }
}
