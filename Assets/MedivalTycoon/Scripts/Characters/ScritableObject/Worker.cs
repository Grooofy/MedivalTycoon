using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Workers", order = 41)]
    public class Worker : ScriptableObject
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _id;
        [SerializeField] private int _numberWearableObjects;
        [SerializeField] private float _distanceBetweenPoints;
    
        public bool IsSelect;
    
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotateSpeed;
        public int NumberWearableObjects => _numberWearableObjects;
        public float DistanceBetweenPoints => _distanceBetweenPoints;
        public Sprite Icon => _icon;
        public int Id => _id;
    
        public void ChangeValueSelect()
        {
            IsSelect = !IsSelect;
        }
    }
}

