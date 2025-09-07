using System.Collections;
using System.Collections.Generic;
using Propses;
using UnityEngine;

public class Beer : MonoBehaviour, IProps
{
    private TransformMover _mover = new TransformMover();
    private Transform _startPoint;
  
    private float _moveSpeed;
    private bool _isFirstAnimationPlaying;
    
    public void Initilization(Transform parent, float moveSpeed, Animator animator)
    {
      
        _moveSpeed =  moveSpeed;
        _startPoint = parent;
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

    public void Reset()
    {
        transform.position = _startPoint.position;
        _isFirstAnimationPlaying = false;
    }
}
