using UnityEngine;
using UnityEngine.Serialization;

namespace Lever
{
    public class LeverInstaller : MonoBehaviour
    {
        [Header("LeverBarrel")] 
        [SerializeField] private SphereCollider _barrelCollider;
        [SerializeField] private Animator _barrelAnimator;
        [SerializeField] private GroundUI _barrelGroundUI;
        [SerializeField] private LeverGetBarrel _leverGetBarrel;
        private LeverBarrelAnimator _leverBarrelAnimator;

        [Header("LeverBarrelToBeer")] 
        [SerializeField] private SphereCollider _beerCollider;
        [SerializeField] private Animator _beerAnimator;
        [SerializeField] private GroundUI _beerGroundUI;
        [SerializeField] private LeverGetBarrel _leverGetBeer;

        public void Initialize(IPropsMover propsMover)
        {
            _leverBarrelAnimator = new LeverBarrelAnimator(_barrelAnimator, false);
            _barrelGroundUI.Initialize();
            _leverGetBarrel.Initialize(propsMover, _barrelCollider, _barrelGroundUI);
            
        }

        private void OnDestroy()
        {
            _leverBarrelAnimator.OnDestroy();
        }
    }
}