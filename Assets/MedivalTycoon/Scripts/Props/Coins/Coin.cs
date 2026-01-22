using Barrels;
using Propses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Money
{
    public class Coin : MonoBehaviour, IProps
    {
        private TransformMover _mover = new TransformMover();
        private Transform _startPoint;
        private CoinAnimation _barrelAnimation;
        private float _moveSpeed;

        public void Initilization(Transform parent, float moveSpeed, Animator animator)
        {
            _barrelAnimation = new CoinAnimation(animator);
            _moveSpeed = moveSpeed;
            _startPoint = parent;
        }

        public void Reset()
        {
            transform.position = _startPoint.position;
            _barrelAnimation.Reset();
        }

        public IEnumerator TryMoveTo(Point endPoint)
        {
            if (endPoint.IsFill) yield break;

            while (endPoint.IsFill == false)
            {
                _mover.MoveTo(transform, endPoint, _moveSpeed);
                yield return null;
            }           
        }

    }
}