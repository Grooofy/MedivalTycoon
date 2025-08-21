using UnityEngine;

namespace Visitors
{
    public class IdleState : IVisitorsState
    {
        private TavernVisitor _tavernVisitor;

        public IdleState(TavernVisitor tavernVisitor)
        {
            _tavernVisitor = tavernVisitor;
        }
        
        public void Enter()
        {
            _tavernVisitor.SetRandomAmountBeer();
            AnimatorExtensions.Play(_tavernVisitor.Animator, AnimatorParameters.VisitorIdle);
        }

        public void UpdateState()
        {
            Debug.Log("Idle state");
        }

        public void Exit()
        {
           
        }
    }

}