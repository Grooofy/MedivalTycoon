using UnityEngine;

namespace Lever
{
    public class LeverInstaller : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private GroundUI _groundUI;
        [SerializeField] private LeverGetBarrel _leverGetBarrel;

        private LeverBarrelAnimator _leverBarrelAnimator;

        public void Initialize(IPropsMover propsMover)
        {
            _leverBarrelAnimator = new LeverBarrelAnimator(_animator, false);
            _groundUI.Initialize();
            _leverGetBarrel.Initialize(propsMover, _collider, _groundUI);
        }

        private void OnDestroy()
        {
            _leverBarrelAnimator.OnDestroy();
        }
    }
}