using UnityEngine;
using System.Collections;
using Barrels;
using Propses;


public class Barrel : MonoBehaviour, IProps
{
    private TransformMover _mover = new TransformMover();
    private Transform _startPoint;
    private BarrelAnimation _barrelAnimation;
    private float _moveSpeed;
    private bool _isFirstAnimationPlaying;

    //public GameObject Prefab { get; }

    public void Initilization(Transform parent, float moveSpeed, Animator animator)
    {
        _barrelAnimation = new BarrelAnimation(animator);
        _moveSpeed =  moveSpeed;
        _startPoint = parent;
    }

    public void Reset(IPropsMover propsMover, Point point)
    {
        point.Free();
        transform.SetParent(_startPoint);
        transform.position = _startPoint.position;
        _barrelAnimation.Reset();
        _isFirstAnimationPlaying = false;
        //propsMover.RegisterProp(this);
    }
    
    public IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint.IsFill) yield break;
        
        while (endPoint.IsFill == false)
        {
            _mover.MoveTo(transform, endPoint, _moveSpeed);
            yield return null;
        }

        if (_isFirstAnimationPlaying == false)
        {
            _barrelAnimation.MoveEnd();
            _isFirstAnimationPlaying = true;
        }
    }

   
}