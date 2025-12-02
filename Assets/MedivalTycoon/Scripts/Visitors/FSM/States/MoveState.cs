using Unity.VisualScripting;
using UnityEngine;

namespace Visitors
{
    public class MoveState : IVisitorsState
    {
        private readonly TavernVisitor _tavernVisitor;
        public MoveState(TavernVisitor tavernVisitor)
        {
            _tavernVisitor = tavernVisitor;           
        }

        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorWalk);           
        }

        public void UpdateState()
        {
            _tavernVisitor.MoveToPoint();
            Debug.Log("Move state");
        }

        public void Exit()
        {
        }
    }
}