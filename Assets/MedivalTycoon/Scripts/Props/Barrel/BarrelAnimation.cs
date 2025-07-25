using UnityEngine;

namespace Barrels
{
    public class BarrelAnimation 
    {
        private Animator _animator;

        public BarrelAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void MoveEnd()
        {
            Debug.Log("Animation End!!!!!");
            AnimatorExtensions.Set(_animator, AnimatorParameters.MoveEnd);
        }

        public void Reset()
        {
            AnimatorExtensions.Set(_animator, AnimatorParameters.Reset);
        }
    }
}