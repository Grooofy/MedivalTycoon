using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, ICharacter
    {
        private Worker _worker;
        private CharacterController _controller;
        private Animator _animator;
   

        public void Initialize(Worker worker)
        {
            _worker = worker;
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        }

        public int GetId()
        {
            return _worker.Id;
        }
   
        public int GetNumberWearableObjects()
        {
            return _worker.NumberWearableObjects;
        }

        public void Move(Vector3 direction)
        {
            TryRotate(direction);
            var normalizeDirection = Vector3.Normalize(direction);
            _controller.Move(normalizeDirection * _worker.Speed * Time.deltaTime);
            _animator.SetFloat("Speed", _controller.velocity.magnitude);
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
