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
            AnimatorExtensions.Set(_animator, AnimatorParameters.Reset);
        }
    }
}