using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    public class SwitcherSelectedCharacter : MonoBehaviour
    {
        private ButtonsTransmitter _buttonsTransmitter;
        private CharacterInputController _characterInput;
        private List<ICharacter> _characters = new List<ICharacter>();
        private ICharacter _currentCharacter;
    
        public UnityAction<int> Activate;
        
        public void Initialize(ButtonsTransmitter activeCharacter, CharacterInputController characterInput, List<ICharacter> characters)
        {
            _buttonsTransmitter = activeCharacter;
            _characterInput = characterInput;
            _characters = characters;
            _buttonsTransmitter.CharacterSelected += InitSelectedCharacter;
        }
        
        private void OnDisable()
        {
            _buttonsTransmitter.CharacterSelected -= InitSelectedCharacter;
        }
        
        private void InitSelectedCharacter(int id)
        {
            _currentCharacter = _characters[id];
            _characterInput.SwitchCharacter(_currentCharacter);
            Activate?.Invoke(id);
        }
    }
}
