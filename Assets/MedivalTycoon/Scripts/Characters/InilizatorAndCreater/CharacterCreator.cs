using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class CharacterCreator : MonoBehaviour
    {
        [SerializeField] private List<Character> _characterPrefabs;
        [SerializeField] private List<Worker> _workers;
        [SerializeField] private List<Transform> _spawnPoints;
       
        
        public ICharacter StartSelectedCharacter { get; private set; }
        public readonly List<ICharacter> Characters = new List<ICharacter>();
        private InputInitializer _inputInitializer = new InputInitializer();
        
        
        public void Create(IInputSystem controller, FloatingJoystick joystick)
        {
            for (int i = 0; i < _characterPrefabs.Count; i++)
            {
                ICharacter character = Instantiate(_characterPrefabs[i], _spawnPoints[i]);
                character.Initialize(_workers[i]);
                
                if (_workers[i].IsSelect)
                {
                    StartSelectedCharacter = character;
                }
                character.HandTool.CreatePoints(character.GetNumberWearableObjects(), character.GetDistanceBetweenPoints());
                Characters.Add(character);
            }
            _inputInitializer.InitializeInputController(controller, Characters,  joystick, StartSelectedCharacter);
        }
    }
}