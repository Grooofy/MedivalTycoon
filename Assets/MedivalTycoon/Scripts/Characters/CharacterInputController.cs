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
       
        
        public void Initialize(List<ICharacter> characters, FloatingJoystick joystick, ICharacter currentCharacter = null)
        {
            _characters = characters;
            _currentCharacter = currentCharacter;
            _joystick = joystick;
            InitializeCosAndSin();
        }

        public void SwitchCharacter(ICharacter character)
        {
            _currentCharacter = character;
        }
    
        private void InitializeCosAndSin()
        {
            _cosX = Mathf.Cos(_angelOffset);
            _sinX = Mathf.Sin(_angelOffset);
        }
    
        public void ReadMoveDirection()
        {
            if (_joystick is not null && _joystick.isActiveAndEnabled)
            {
                ReadMove();
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
