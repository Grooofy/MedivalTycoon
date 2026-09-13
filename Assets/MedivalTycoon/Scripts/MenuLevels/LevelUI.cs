using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Button _button;
    [SerializeField] private List<Image> _coinImages;
    [SerializeField] private LevelData _levelData;

    private Level _level;

    private void OnEnable()
    {
        _button.onClick.AddListener(SelectLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SelectLevel);
    }

    public void SetLevel(Level level)
    {
        _level = level;
        PaintCoins(LevelRewards.GetBest(level.NumberLevel));
    }

    public void SelectLevel()
    {
        if (_level != null)
            _levelData.Save(_level);
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
    }

    public void SwitchButtonInteractable()
    {
        _button.interactable = !_button.interactable;
    }

    public void PaintCoins(int countCoins)
    {
        for (int i = 0; i < _coinImages.Count; i++)
        {
            Color color = _coinImages[i].color;
            color.a = i < countCoins ? 1f : 0.2f;
            _coinImages[i].color = color;
        }
    }

    public void ShowNumber(int number)
    {
        _textMesh.text = number.ToString();
    }

}
