using Events;
using UnityEngine;

namespace Lever
{
    public class LeverBeerAnimation
    {
        private Animator _animator;
        private bool _isActive;

        public LeverBeerAnimation(Animator animator, bool isActive)
        {
            _isActive = isActive;
            _animator = animator;
            EventBus.Subscribe<BeerBufferOpen>(ChangeValue);
        }

        public void OnDestroy()
        {
            EventBus.Unsubscribe<BeerBufferOpen>(ChangeValue);
        }

        private void ChangeValue(BeerBufferOpen value)
        {
            _isActive = !_isActive;
            AnimatorExtensions.Set(_animator, AnimatorParameters.LeverIsOn, _isActive);
        }
    }
}