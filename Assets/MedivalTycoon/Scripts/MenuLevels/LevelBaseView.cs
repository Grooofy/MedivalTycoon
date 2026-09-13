using UnityEngine;
public class LevelBaseView : MonoBehaviour
{
    [SerializeField] private LevelButtonCreater _levelButtonCreator;

    private void Start()
    {
        _levelButtonCreator.Initialize();
        ShowIcons();
    }

    private void ShowIcons()
    {
        int iconCount = _levelButtonCreator.GetIconsCount();

        for (int i = 0; i < iconCount; i++)
        {
            if (i == 0)
            {
                ShowButtonInteractable(i);
            }
            else
            {
                ShowIconText(i);
                _levelButtonCreator.GetLevelButton(i).SetInteractable(
                    _levelButtonCreator.GetInfoCompleted(i - 1));
            }
        }
    }

    private void ShowIconText(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).ShowNumber(sequenceNumber);
    }

    private void ShowButtonInteractable(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).SetInteractable(true);
    }

}
