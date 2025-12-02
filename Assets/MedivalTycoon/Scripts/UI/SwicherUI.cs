using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.Serialization;

public class SwicherUI : MonoBehaviour
{
    [FormerlySerializedAs("_transmitter")] [SerializeField] private ButtonsTransmitter buttonsTransmitter;
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
