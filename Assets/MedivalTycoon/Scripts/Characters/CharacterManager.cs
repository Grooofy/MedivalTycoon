using UnityEngine;

namespace Characters
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField] private CharacterCreator _characterCreator;
        [SerializeField] private CameraFollower _cameraFollower;
        [SerializeField] private CharacterInputController _characterInputController;
        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private ButtonsTransmitter _buttonsTransmitter;
        [SerializeField] private SwitcherSelectedCharacter _switcherSelectedCharacter;

        private void Start()
        {
            _characterCreator.Create(_characterInputController, _joystick);
            _buttonsTransmitter.Initialize();
            _switcherSelectedCharacter.Initialize(_buttonsTransmitter, _characterInputController, _characterCreator.Characters);
            _cameraFollower.Initialize(_characterCreator.Characters, _switcherSelectedCharacter, _characterCreator.StartSelectedCharacter);
        }

        private void Update()
        {
            _characterInputController.ReadMoveDirection();
            _cameraFollower.Move();
        }

        private void OnDestroy()
        {
            _cameraFollower.Dispose();
        }
    }
}