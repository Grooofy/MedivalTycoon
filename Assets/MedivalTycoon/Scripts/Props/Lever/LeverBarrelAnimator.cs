using UnityEngine;

namespace Lever
{
    public class LeverBarrelAnimator
    {
        private Animator _animator;

        public LeverBarrelAnimator(Animator animator)
        {
            _animator = animator;
            EventBus.Subscribe<PropsMoverFullingPointEvent>(ChangeValue);
        }

        public void OnDestroy()
        {
            EventBus.Unsubscribe<PropsMoverFullingPointEvent>(ChangeValue);
        }

        private void ChangeValue(PropsMoverFullingPointEvent value)
        {
            AnimatorExtensions.Set(_animator, AnimatorParameters.LeverIsOn, value.IsFull);
        }
    }
}