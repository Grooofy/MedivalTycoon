using Propses;
using System.Collections;
using UnityEngine;
using Visitors;

public class VisitorProps : MonoBehaviour, IProps
{
    private TavernVisitor _visitor;
    private TransformMover _mover = new TransformMover();
    private float _moveSpeed;

    public void Initilization(Transform spawnPoint, float moveSpeed, Animator animator)
    {
        _moveSpeed = moveSpeed;
    }

    public void SetVisitor(TavernVisitor visitor)
    {
        _visitor = visitor;        
    }

    public IEnumerator TryMoveTo(Point endPoint)
    {
        if (endPoint.IsFill) yield break;
        while (endPoint.IsFill == false)
        {
            _mover.MoveTo(transform, endPoint, _moveSpeed);
            AnimatorExtensions.Set(_visitor.Animator, AnimatorParameters.VisitorMoveSleep);
            yield return null;
        }
    }
    public void Reset()
    {
        _visitor.ClearPoint();
        gameObject.SetActive(false);
    }
}
