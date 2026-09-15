using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ICharacter
    {
        public IPropsMover HandTool => _hand;
        
        private Worker _worker;
        private float _moveSpeed;
        private float _rotationSpeed;
        private int _capacity;
        private CharacterController _controller;
        private Animator _animator;
        private IPropsMover _hand;
        
        private Vector3 _velocity;
   

        public void Initialize(Worker worker)
        {
            _worker = worker;
            _moveSpeed = CharacterUpgrades.GetValue(worker, CharacterUpgrades.Stat.Speed);
            float speedMultiplier = worker.MoveSpeed > 0f ? _moveSpeed / worker.MoveSpeed : 1f;
            _rotationSpeed = worker.RotationSpeed * speedMultiplier;
            _capacity = Mathf.RoundToInt(CharacterUpgrades.GetValue(worker, CharacterUpgrades.Stat.Capacity));
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _hand = GetComponentInChildren<IPropsMover>();
        }
   
        public int GetNumberWearableObjects()
        {
            return _capacity;
        }

        public float GetDistanceBetweenPoints()
        {
            return _worker.DistanceBetweenPoints;
        }


        public void Move(Vector3 direction)
        {
            TryRotate(direction);
            var normalizeDirection = Vector3.Normalize(direction);
            MoveController(normalizeDirection);
            _animator.SetFloat("Speed", _controller.velocity.magnitude);
        }
        
        private void MoveController(Vector3 normalizeDirection)
        {
            _controller.Move((normalizeDirection + _velocity) * _moveSpeed * Time.deltaTime);
            
            if (!_controller.isGrounded)
                _velocity.y += -9.81f * Time.deltaTime;
            else if (_velocity.y < 0)
                _velocity.y = -2f;
            
            Vector3.Normalize(_velocity);
        }
        
        public Vector3 GetPosition()
        {
            return _controller.transform.position;
        }

        private void TryRotate(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}
