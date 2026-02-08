using Propses;
using System.Collections;
using UnityEngine;

public class VisitorProps : MonoBehaviour, IProps
{
    private TransformMover _mover = new TransformMover();
    private float _moveSpeed;

    public void Initilization(Transform spawnPoint, float moveSpeed, Animator animator)
    {
        _moveSpeed = moveSpeed;
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
       
    }
}
