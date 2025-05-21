using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverStoper : MonoBehaviour
{
    [SerializeField] CharacterInputController _inputController;


    public void TurnOffMove()
    {
        _inputController.IsStop = true;
    }

    public void TurnOnMove()
    {
        _inputController.IsStop = false;
    }
}
