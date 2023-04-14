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
        ShowButtonInteractable(0);
        
        for (int i = 0; i < _levelButtonCreator.GetIconsCount(); i++)
        {
            ShowIconText(i);
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