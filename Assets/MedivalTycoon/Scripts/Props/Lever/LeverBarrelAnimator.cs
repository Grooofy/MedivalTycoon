using UnityEngine;

namespace Lever
{
    public class LeverBarrelAnimator
    {
        private Animator _animator;
        private bool _isActive;

        public LeverBarrelAnimator(Animator animator, bool isActive)
        {
            _animator = animator;
            _isActive = isActive;
            EventBus.Subscribe<PropsMoverFullingPointEvent>(ChangeValue);
        }

        public void OnDestroy()
        {
            EventBus.Unsubscribe<PropsMoverFullingPointEvent>(ChangeValue);
        }

        private void ChangeValue(PropsMoverFullingPointEvent value)
        {
            if (value.SourceId != "Barrel") return;
            
            _isActive = !_isActive;
            AnimatorExtensions.Set(_animator, AnimatorParameters.LeverIsOn, _isActive);
        }
    }
}