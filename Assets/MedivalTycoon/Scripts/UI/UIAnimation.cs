using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimation : MonoBehaviour
{
    [Header("Buttons (from top to bottom on the chain)")]
    [SerializeField] private List<RectTransform> _buttons = new List<RectTransform>();

    [Header("Drop settings")]
    [SerializeField] private float _dropHeight = 250f; // pixels above original
    [SerializeField] private float _dropDuration = 0.7f;
    [SerializeField] private float _stagger = 0.06f;
    [SerializeField] private Ease _dropEase = Ease.OutBack;

    [Header("Lift on pause (up->down)")]
    [SerializeField] private float _liftAmount = 40f; // pixels to lift on pause
    [SerializeField] private float _liftUpDuration = 0.18f;
    [SerializeField] private float _liftDownDuration = 0.28f;
    [SerializeField] private float _liftStagger = 0.03f;
    [SerializeField] private Ease _liftUpEase = Ease.OutSine;
    [SerializeField] private Ease _liftDownEase = Ease.InQuad;

    private List<Vector2> _initialAnchored = new List<Vector2>();
    private List<Quaternion> _initialRotations = new List<Quaternion>();
    private bool _initialized;
    private bool _isPlaying;

    

    // Cache initial positions and rotations
    public void Initialize()
    {
        _initialAnchored.Clear();
        _initialRotations.Clear();

        foreach (var rt in _buttons)
        {
            if (rt == null) continue;
            _initialAnchored.Add(rt.anchoredPosition);
            _initialRotations.Add(rt.localRotation);
        }

        _initialized = true;
    }

    // Запустить анимацию падения и последующего покачивания
    public void Play()
    {
        if (!_initialized) Initialize();
        if (_isPlaying) return;
        _isPlaying = true;

        for (int i = 0; i < _buttons.Count; i++)
        {
            var rt = _buttons[i];
            if (rt == null) continue;

            // Остановить предыдущие твины для этого объекта
            DOTween.Kill(rt);

            var targetPos = _initialAnchored[i];
            // Поставить выше на dropHeight
            rt.anchoredPosition = targetPos + Vector2.up * _dropHeight;
            rt.localRotation = _initialRotations[i];

            float delay = i * _stagger;

            // Последовательность: падение
            var seq = DOTween.Sequence();
            seq.Append(rt.DOAnchorPos(targetPos, _dropDuration).SetDelay(delay).SetEase(_dropEase));
            seq.SetId(rt);
            seq.Play();
        }

        // Снять флаг isPlaying после окончания всех падений
        float totalDuration = _dropDuration + Mathf.Max(0, (_buttons.Count - 1) * _stagger);
        DOVirtual.DelayedCall(totalDuration + 0.02f, () => _isPlaying = false).SetId(this);
    }

    // Остановить анимации и вернуть в исходное состояние
    public void ResetAnimation()
    {
        _isPlaying = false;
        for (int i = 0; i < _buttons.Count; i++)
        {
            var rt = _buttons[i];
            if (rt == null) continue;
            DOTween.Kill(rt);
            rt.anchoredPosition = _initialAnchored[Mathf.Clamp(i, 0, _initialAnchored.Count - 1)];
            rt.localRotation = _initialRotations[Mathf.Clamp(i, 0, _initialRotations.Count - 1)];
        }
    }

    // Запускает бесконечное покачивание с небольшой амплитудой, отличающейся для каждой кнопки
    // Поднять UI (например при нажатии Pause) — все кнопки поднимаются и потом возвращаются
    public void Lift()
    {
        if (!_initialized) Initialize();

        for (int i = 0; i < _buttons.Count; i++)
        {
            var rt = _buttons[i];
            if (rt == null) continue;

            DOTween.Kill(rt);

            var basePos = _initialAnchored[Mathf.Clamp(i, 0, _initialAnchored.Count - 1)];
            float delay = i * _liftStagger;

            var seq = DOTween.Sequence();
            seq.Append(rt.DOAnchorPos(basePos + Vector2.up * _liftAmount, _liftUpDuration).SetEase(_liftUpEase));
            seq.Append(rt.DOAnchorPos(basePos, _liftDownDuration).SetEase(_liftDownEase));
            seq.SetDelay(delay).SetId(rt).Play();
        }
    }
}
