using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ICharacter
    {
        public IPropsMover HandTool => _hand;
        
        private Worker _worker;
        private CharacterController _controller;
        private Animator _animator;
        private IPropsMover _hand;
        
        private Vector3 _velocity;
   

        public void Initialize(Worker worker)
        {
            _worker = worker;
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _hand = GetComponentInChildren<IPropsMover>();
        }
   
        public int GetNumberWearableObjects()
        {
            return _worker.NumberWearableObjects;
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
            _controller.Move((normalizeDirection + _velocity) * _worker.Speed * Time.deltaTime);
            
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
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
