using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Visitors : MonoBehaviour
{
    [SerializeField] private LoadingGameSettings _loadingGameSettings;
    [SerializeField] private TextMeshProUGUI _gameMoney;

    private void Start()
    {
        ShowTextVisitors();
    }

    private void ShowTextVisitors()
    {
        _gameMoney.text = _loadingGameSettings.GetVisitors().ToString();
    }
}
