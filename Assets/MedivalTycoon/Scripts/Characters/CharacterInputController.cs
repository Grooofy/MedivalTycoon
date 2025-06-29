using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class CharacterInputController : MonoBehaviour, IInputSystem
    {
        public bool IsStop;
        
        private Vector3 _moveDirection;
        private Animator _animator;
        private FloatingJoystick _joystick;
        private List<ICharacter> _characters = new List<ICharacter>();
        private ICharacter _currentCharacter;
        private float _angelOffset = 70;
        private float _cosX;
        private float _sinX;
        private float _y;
        
        public void Initialize(List<ICharacter> characters, FloatingJoystick joystick, ICharacter currentCharacter = null)
        {
            _characters = characters;
            _currentCharacter = currentCharacter;
            _joystick = joystick;
            _y = transform.position.y;
        }

        public void SwitchCharacter(ICharacter character)
        {
            _currentCharacter = character;
        }
    
        private void Start()
        {
            _cosX = Mathf.Cos(_angelOffset);
            _sinX = Mathf.Sin(_angelOffset);
        }
    
        public void ReadMoveDirection()
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
            _currentCharacter.Move(_moveDirection);
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
}
