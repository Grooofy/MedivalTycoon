using UnityEngine;

public class MoverStoper : MonoBehaviour
{
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] private Character _character;


    public void TurnOffMove()
    {
        _inputController.enabled = false;
        _character.IsCanMove = false;
    }

    public void TurnOnMove()
    {
        _inputController.enabled = true;
        _character.IsCanMove = true;
    }
}
