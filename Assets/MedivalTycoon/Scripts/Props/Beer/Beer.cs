using System.Collections;
using System.Collections.Generic;
using Propses;
using UnityEngine;

public class Beer : MonoBehaviour, IProps
{
    private TransformMover _mover = new TransformMover();
    private Transform _startPoint;
    private Animator _animator;
  
    private float _moveSpeed;
    
    
    public void Initilization(Transform parent, float moveSpeed, Animator animator)
    {
        _startPoint = parent;
        _moveSpeed =  moveSpeed;
        _animator = animator;       
    }

    public void Reset()
    {
        transform.position = _startPoint.position;
        AnimatorExtensions.Set(_animator, AnimatorParameters.ResetBeer);
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
