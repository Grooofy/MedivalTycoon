using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public interface IInputSystem
    {
        public void Initialize(List<ICharacter> characters, FloatingJoystick joystick, ICharacter currentCharacter = null);
    }
}