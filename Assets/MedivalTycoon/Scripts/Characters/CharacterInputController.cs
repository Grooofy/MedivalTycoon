using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private float _angelOffset = 70;
    [SerializeField] private Animator _animator;

    public bool IsStop;
    
    private Vector3 _moveDirection;
    private ICharacter _character;
    private float _cosX;
    private float _sinX;
    private float _y;
    
    private void Awake()
    {
        _character = GetComponent<ICharacter>();
        _y = transform.position.y;
        
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
        if (_joystick.isActiveAndEnabled)
        {
            ReadMove();
            CheckGround();
        }
    }

    private void ReadMove()
    {
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;
        
        float newHorizontal = CalculateOffSetX(horizontal, vertical);
        float newVertical = CalculateOffSetY(horizontal, vertical);

        _moveDirection = new Vector3(newHorizontal , 0, newVertical);
        _character.Move(_moveDirection);
        _animator.SetFloat("Speed", _moveDirection.magnitude);
    }

    private void CheckGround()
    {
        if (transform.position.y > _y)
        {
            transform.position = new Vector3(transform.position.x, _y, transform.position.z);
        }
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