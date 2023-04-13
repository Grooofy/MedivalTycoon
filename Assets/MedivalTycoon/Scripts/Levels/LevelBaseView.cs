using UnityEngine;
public class LevelBaseView : MonoBehaviour
{
    [SerializeField] private LevelButtonCreater _levelButtonCreator;
    [SerializeField] private LevelBase _levelBase;
    
    private void Start()
    {
        ShowIcons();
    }

    private void ShowIcons()
    {
        SwitchButtonInteractable(0);
        
        for (int i = 0; i < _levelBase.LevelsCount; i++)
        {
            ShowIconText(i);
        }
    }

    private void ShowIconText(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).ShowNumber(sequenceNumber);
    }

    private void SwitchButtonInteractable(int sequenceNumber)
    {
        _levelButtonCreator.GetLevelButton(sequenceNumber).SwitchButtonInteractable();
    }

    

    
}