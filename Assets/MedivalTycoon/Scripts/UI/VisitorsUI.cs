using UnityEngine;
using TMPro;
using Events;

public class VisitorsUI : MonoBehaviour
{
    private LoadingGameSettings _loadingGameSettings;
    private TextMeshProUGUI _visitorsText;
    private int _visitorsAmount;
    

    public void Initialize(LoadingGameSettings loadingGameSettings)
    {
        _visitorsAmount = loadingGameSettings.GetVisitors();
        _visitorsText = GetComponentInChildren<TextMeshProUGUI>();
        ShowTextVisitors();
        EventBus.Subscribe<VisitorLeaveTavern>(UpdateTextVisitors);
    }

    private void ShowTextVisitors()
    {
        _visitorsText.text = _visitorsAmount.ToString();
    }

    private void UpdateTextVisitors(VisitorLeaveTavern visitorLeaveTavern)
    {
        _visitorsText.text = (_visitorsAmount -= 1).ToString();
    }
}
