using Beers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class BarrelCountUI : MonoBehaviour
{
    [SerializeField] private BarrelBeerBuffer _buffer;
    private TMP_Text _text;
    private Text _legacyText;
    private Canvas _canvas;
    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>(true);
        _legacyText = GetComponentInChildren<Text>(true);
        _canvas = GetComponent<Canvas>();
    }
    private void LateUpdate()
    {
        var camera = _canvas != null && _canvas.worldCamera != null ? _canvas.worldCamera : Camera.main;
        if (camera != null) transform.rotation = camera.transform.rotation;
        string count = (_buffer != null ? _buffer.AvailableCount : 0).ToString();
        if (_text != null) _text.text = count;
        if (_legacyText != null) _legacyText.text = count;
    }
}
