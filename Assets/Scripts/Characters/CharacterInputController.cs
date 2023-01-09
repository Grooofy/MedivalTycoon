using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Worker _workerObject;
    
    private ICharacter _character;
   

    private void Awake()
    {
        _character = GetComponent<ICharacter>();
        
        if (_character == null)
        {
            throw new Exception($"There is no ICharacter component on the object: {gameObject.name}");
        }
    }

    private void Update()
    {
        ReadMove();
    }

    private void ReadMove()
    {
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        _character.Move(direction, _workerObject.Speed);
    }
}