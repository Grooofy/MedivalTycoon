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
        _button.onClick.AddListener(delegate { _levelData.Save(_level); });
    }

    public void SetLevel(Level level)
    {
        _level = level;
    }
   
    public void SwitchButtonInteractable()
    {
        _button.interactable = !_button.interactable;
    }

    public void PaintCoins(int countCoins)
    {
        switch (countCoins)
        {
                case 1:
                    PaintCoin(_coinImages[0]);
                    break;
                
                case 2:
                    for (int i = 0; i < 2; i++)
                        PaintCoin(_coinImages[i]);
                    break;
                
                case 3:
                    foreach (var coinImage in _coinImages)
                        PaintCoin(coinImage);
                    break;
        }
    }
    
    public void ShowNumber(int number)
    {
        number++;
        _textMesh.text = number.ToString();
    }

    private void PaintCoin(Image coinImage)
    {
        const int newAlfaChannel = 255;
        Color color = coinImage.color;
        color.a = newAlfaChannel;
        coinImage.color = color;
    }
}
