using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUITutorial : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private List<Image> _coinImages;
    [SerializeField] private LevelData _levelData;

    private Level _level;

    private void OnEnable()
    {
        _button.onClick.AddListener(delegate { _levelData.Save(_level); });
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(delegate { _levelData.Save(_level); });
    }

    public void SetLevel(Level level)
    {
        _level = level;
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

    private void PaintCoin(Image coinImage)
    {
        const int newAlfaChannel = 255;
        Color color = coinImage.color;
        color.a = newAlfaChannel;
        coinImage.color = color;
    }
}
