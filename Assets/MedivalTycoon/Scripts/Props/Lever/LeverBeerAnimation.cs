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
            EventBus.Subscribe<PropsMoverFullingPointEvent>(ChangeValue);
        }

        public void OnDestroy()
        {
            EventBus.Unsubscribe<PropsMoverFullingPointEvent>(ChangeValue);
        }

        private void ChangeValue(PropsMoverFullingPointEvent value)
        {
            _isActive = !_isActive;
            AnimatorExtensions.Set(_animator, AnimatorParameters.LeverIsOn, _isActive);
        }
    }
}