using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class InputInitializer
    {
        public void InitializeInputController(IInputSystem controller, List<ICharacter> characters, FloatingJoystick joystick, ICharacter currentCharacter = null)
        {
            controller.Initialize(characters, joystick, currentCharacter);
        }
    }
}