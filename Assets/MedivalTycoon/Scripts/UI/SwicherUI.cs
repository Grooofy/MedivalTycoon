using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwicherUI : MonoBehaviour
{
    [SerializeField] private Transmitter _transmitter;
    [SerializeField] private List<GroundUI> _groundsUI;

    private GroundUI _activeGroundUI;

    private void OnEnable()
    {
        _activeGroundUI = _groundsUI[0];
    }

    private void SwitchUI(int id)
    {
               
    }
}
