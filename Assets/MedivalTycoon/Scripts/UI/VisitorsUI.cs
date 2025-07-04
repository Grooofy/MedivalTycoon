using UnityEngine;
using TMPro;

public class VisitorsUI : MonoBehaviour
{
    private LoadingGameSettings _loadingGameSettings;
    private TextMeshProUGUI _visitorsText;
    private int _visitorsAmount;
    

    public void Initialize(LoadingGameSettings loadingGameSettings)
    {
        _visitorsAmount = loadingGameSettings.GetVisitors();
        _visitorsText = GetComponentInChildren<TextMeshProUGUI>();
        
        ShowTextVisitors();//Временно, пока не обновлен обработчик гостей
    }

    private void ShowTextVisitors()
    {
        _visitorsText.text = _visitorsAmount.ToString();
    }
}
