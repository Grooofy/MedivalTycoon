using UnityEngine;

namespace Lever
{
    public class LeverAnimator
    {
        private const string ParameterName = "IsOn";
        private Animator _animator;

        public LeverAnimator(Animator animator)
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
            if (value.SourceId != "Barrel") return;
            
            _animator.SetBool(ParameterName, value.IsFull);
        }
    }
}