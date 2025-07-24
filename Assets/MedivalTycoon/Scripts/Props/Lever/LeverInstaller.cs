using System;
using Propses;
using UnityEngine;

namespace Lever
{
    public class LeverInstaller : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private GroundUI _groundUI;
        [SerializeField] private LeverGetBarrel _leverGetBarrel;

        private LeverAnimator _leverAnimator;

        public void Initialize(IPropsMover propsMover)
        {
            _leverAnimator = new LeverAnimator(_animator);
            _groundUI.Initialize();
            _leverGetBarrel.Initialize(propsMover, _collider, _groundUI);
        }

        private void OnDestroy()
        {
            _leverAnimator.OnDestroy();
        }
    }
}