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
        [SerializeField] private LeverBeer leverBeer;
        private LeverBeerAnimator _leverBeerAnimator;

        public void InitializeBarrelLever(IPropsMover propsMover)
        {
            _leverBarrelAnimator = new LeverBarrelAnimator(_barrelAnimator, false);
            _barrelGroundUI.Initialize();
            _leverGetBarrel.Initialize(propsMover, _barrelCollider, _barrelGroundUI);
        }

        public void InitializeBeerLever(IPropsMover propsMover)
        {
            _leverBeerAnimator = new LeverBeerAnimator(_beerAnimator, false);
            _beerGroundUI.Initialize();
            leverBeer.Initialize(propsMover,  _beerCollider, _beerGroundUI);
        }

        private void OnDestroy()
        {
            _leverBarrelAnimator.OnDestroy();
            _leverBeerAnimator.OnDestroy();
        }
    }
}