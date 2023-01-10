using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterInputController))]
public class SwitchInputController : MonoBehaviour
{
    [SerializeField] private ActiveCharacter _activeCharacter;
    [SerializeField] private CharacterInputController _characterInput;
    [SerializeField] private Character _character;
    
    public UnityAction<int> Activate;

    private int _myId;

    private void OnEnable()
    {
        _myId = _character.GetId();
        _activeCharacter.CharacterSelected += Switch;
    }

    private void OnDisable()
    {
        _activeCharacter.CharacterSelected -= Switch;
    }

    private void Switch(int id)
    {
        Switching(_myId == id);
    }

    private void Switching(bool isValue)
    {
        _characterInput.enabled = isValue;
        
        if (isValue)
        {
            Activate?.Invoke(_myId);
        }
    }
}