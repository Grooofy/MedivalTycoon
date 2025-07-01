using UnityEngine;

namespace Characters
{
    public interface ICharacter
    {
        public void Initialize(Worker worker);
        public IPropsMover HandTool { get; }
        public void Move(Vector3 direction);

        public int GetNumberWearableObjects();
        public float GetDistanceBetweenPoints();
        public Vector3 GetPosition();

    }
}

