using UnityEngine;

namespace Characters
{
    public interface ICharacter
    {
        public void Initialize(Worker worker);
        public void Move(Vector3 direction);
        public Vector3 GetPosition();

    }
}

