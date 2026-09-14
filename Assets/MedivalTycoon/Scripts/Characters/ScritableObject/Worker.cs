using UnityEngine;
using System;

namespace Characters
{
    [CreateAssetMenu(fileName = "Worker", menuName = "Workers", order = 41)]
    public class Worker : ScriptableObject
    {
        [Serializable]
        public struct UpgradeStep
        {
            [Min(1)] public int Price;
            [Min(0.01f)] public float Increase;
        }

        [SerializeField] private UpgradeStep[] _speedUpgrades = Array.Empty<UpgradeStep>();
        [SerializeField] private UpgradeStep[] _capacityUpgrades = Array.Empty<UpgradeStep>();

        public UpgradeStep[] GetUpgrades(CharacterUpgrades.Stat stat) =>
            stat == CharacterUpgrades.Stat.Speed ? _speedUpgrades : _capacityUpgrades;

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

