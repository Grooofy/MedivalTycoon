using UnityEngine;
public class LevelBaseView : MonoBehaviour
{
    [SerializeField] private LevelButtonCreater _levelButtonCreator;

    private void Start()
    {
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
                if (_levelButtonCreator.GetInfoCompleted(i - 1))
                    ShowButtonInteractable(i);
            }
        }
    }

    private void ShowIconText(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).ShowNumber(sequenceNumber);
    }

    private void ShowButtonInteractable(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).SwitchButtonInteractable();
    }

}