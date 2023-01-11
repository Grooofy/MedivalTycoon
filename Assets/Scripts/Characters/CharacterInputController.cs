using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private float _angelOffset = 70;
    
    private ICharacter _character;
    private float _cosX;
    private float _sinX;
    
    private void Awake()
    {
        _character = GetComponent<ICharacter>();
        
        if (_character == null)
            throw new Exception($"There is no ICharacter component on the object: {gameObject.name}");
    }

    private void Start()
    {
        _cosX = Mathf.Cos(_angelOffset);
        _sinX = Mathf.Sin(_angelOffset);
    }

    private void Update()
    {
        ReadMove();
    }

    private void ReadMove()
    {
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;
        
        float newHorizontal = CalculateOffSetX(horizontal, vertical);
        float newVertical = CalculateOffSetY(horizontal, vertical);

        Vector3 direction = new Vector3(newHorizontal , 0f, newVertical);
        _character.Move(direction);
    }

    private float CalculateOffSetX(float x, float y)
    {
        return x * _cosX - y * _sinX;
    }
    
    private float CalculateOffSetY(float x, float y)
    {
        return x * _sinX + y * _cosX;
    }
}