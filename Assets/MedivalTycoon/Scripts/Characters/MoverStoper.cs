using UnityEngine;

public class MoverStoper : MonoBehaviour
{
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] private Character _character;


    public void TurnOffMove()
    {
        _character.IsCantMove = true;
    }

    public void TurnOnMove()
    {
        _character.IsCantMove = false;
    }
}
