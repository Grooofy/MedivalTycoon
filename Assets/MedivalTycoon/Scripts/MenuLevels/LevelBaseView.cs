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
            if (i > 0)
                ShowIconText(i);
            _levelButtonCreator.GetLevelButton(i).SetAvailability(
                _levelButtonCreator.CanStart(i), _levelButtonCreator.GetInfoCompleted(i));
        }
    }

    private void ShowIconText(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).ShowNumber(sequenceNumber);
    }

}
